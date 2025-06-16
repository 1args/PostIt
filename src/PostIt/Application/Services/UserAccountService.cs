using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostIt.Application.Abstractions.Data;
using PostIt.Application.Abstractions.Security.Authentication;
using PostIt.Application.Abstractions.Services;
using PostIt.Common.Abstractions;
using PostIt.Contracts.Exceptions;
using PostIt.Contracts.Requests.User;
using PostIt.Contracts.Responses;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects;

namespace PostIt.Application.Services;

/// <inheritdoc/>
public class UserAccountService(
    IRepository<User> userRepository,
    IRepository<Role> roleRepository,
    IPasswordHasher passwordHasher,
    IAuthenticationService authenticationService,
    IEmailVerificationService emailVerificationService,
    ITransactionManager transactionManager,
    ILogger<UserAccountService> logger) : IUserAccountService
{
    /// <inheritdoc/>
    public async Task<Guid> RegisterAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating user with email `{Email}`.", request.Email);
        
        var name = UserName.Create(request.Name);
        var bio = UserBio.Create(request.Bio);
        var email = UserEmail.Create(request.Email);

        var userExists = await userRepository
            .AsQueryable()
            .AsNoTracking()
            .AnyAsync(u => u.Email == email, cancellationToken);

        if (userExists)
        {
            throw new ConflictException($"User with email {request.Email} already exists.");
        }
        
        var passwordHash = passwordHasher.HashPassword(request.Password);
        var password = UserPassword.Create(passwordHash);
        var user = User.Create(name, bio, email, password, DateTime.UtcNow);

         var role = await roleRepository
            .AsQueryable()
            .SingleOrDefaultAsync(r => r.Id == (int)Domain.Enums.Role.User, cancellationToken)
            ?? throw new InvalidOperationException($"Role {Domain.Enums.Role.User} does not exist.");
         
        user.AddRole(role);

        await transactionManager.StartEffect(async () =>
        {
            await userRepository.AddAsync(user, cancellationToken);
            await emailVerificationService.SendVerificationEmailAsync(user, cancellationToken);
        }, cancellationToken);
        
        logger.LogInformation("User created successfully with ID `{UserId}`.", user.Id);
        
        return user.Id;
    }
    
    /// <inheritdoc/>
    public async Task<AuthResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Login attempt for `{Email}`...", request.Email);

        var email = UserEmail.Create(request.Email);
        
        var user = await userRepository
            .AsQueryable()
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);

        var verifiedPasswordHash = passwordHasher.VerifyHashedPassword(request.Password, user!.Password.Value);

        if (user is null || !verifiedPasswordHash)
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        if (!user.IsEmailConfirmed)
        {
            throw new UnauthorizedException("Please confirm your email to log in to your account.");
        }
        
        var (accessToken, refreshToken) = await authenticationService
            .GenerateAccessAndRefreshTokensAsync(user, cancellationToken);
        
        return new AuthResponse(
            accessToken,
            refreshToken);
    }
    
    /// <inheritdoc/>
    public async Task<bool> VerifyEmailAsync(
        Guid userId,
        Guid token,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Verifying email for user with ID `{UserId}` and token `{Token}`...", userId, token);

        var user = await GetUserAsync(userId, cancellationToken);

        if (user.IsEmailConfirmed)
        {
            logger.LogInformation("Email already confirmed for user {UserId}", userId);
            return false;
        }
        
        var result = await transactionManager.StartEffect(async () =>
        {
            var isVerified = await emailVerificationService.VerifyEmailAsync(user: user, token, cancellationToken);
            
            if (isVerified)
            {
                user.ConfirmEmail();
                await userRepository.UpdateAsync(user, cancellationToken);
                logger.LogInformation("Email verified successfully for user with ID `{UserId}`.", userId);
            }
            
            return isVerified;
        }, cancellationToken);
        return result;
    }
    
    /// <inheritdoc/>
    public async Task LogoutAsync(CancellationToken cancellationToken)
    {
        await authenticationService.RevokeRefreshTokenAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<AuthResponse> RefreshToken(CancellationToken cancellationToken)
    {
        var (accessToken, refreshToken) = await authenticationService.RefreshAccessTokenAsync(cancellationToken);
        return new AuthResponse(
            accessToken,
            refreshToken);
    }
    
    /// <inheritdoc/>
    public async Task RestrictUserAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to restrict user with ID `{UserId}`...", userId);
        
        var currentUserId = GetCurrentUserId();
        
        if (userId == currentUserId)
        {
            logger.LogWarning("Moderator with ID `{UserId}` attempted to restrict themselves.", userId);
            throw new ForbiddenException("Cannot restrict yourself.");
        }
        
        var user = await GetUserAsync(userId, cancellationToken);
        
        if (user.Roles.Any(r => r.Id is (int)Domain.Enums.Role.Moderator or (int)Domain.Enums.Role.Admin))
        {
            logger.LogWarning(
                "Moderator with ID `{ModeratorId}` attempted to restrict a moderator with ID `{UserId}`.", 
                currentUserId,
                userId);
            throw new ForbiddenException("Cannot restrict moderators.");
        }
        
        var (restrictedRole, userRole) = await GetRolesAsync(cancellationToken);

        if (user.Roles.Any(r => r.Id == restrictedRole.Id))
        {
            logger.LogWarning("User with ID `{UserId}` is already restricted.", userId);
            throw new ForbiddenException("User is already restricted.");
        }
        
        await transactionManager.StartEffect(async () =>
        {
            user.AddRole(restrictedRole);
            user.RemoveRole(userRole);
        
            await userRepository.UpdateAsync(user, cancellationToken);
        }, cancellationToken);
        
        logger.LogInformation(
            "User with ID `{UserId}` restricted successfully by moderator `{Moderator}`.", 
            userId,
            currentUserId);
    }
    
    /// <inheritdoc/>
    public async Task UnrestrictUserAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to unrestrict user with ID `{UserId}`...", userId);

        var currentUserId = GetCurrentUserId();
        
        if (userId == currentUserId)
        {
            logger.LogWarning("Moderator with ID `{UserId}` attempted to unrestrict themselves.", userId);
            throw new ForbiddenException("Cannot unrestrict yourself.");
        }
        
        var user = await GetUserAsync(userId, cancellationToken);

        var (restrictedRole, userRole) = await GetRolesAsync(cancellationToken);
        
        var hasRestrictedRole = user.Roles.SingleOrDefault(r => r.Id == restrictedRole.Id);
        
        if (hasRestrictedRole is null)
        {
            logger.LogWarning("User with ID `{UserId}` is not restricted.", userId);
            throw new ConflictException("User is not restricted.");
        }

        await transactionManager.StartEffect(async () =>
        {
            user.RemoveRole(restrictedRole);
            user.AddRole(userRole);
        
            await userRepository.UpdateAsync(user, cancellationToken);
        }, cancellationToken);

        logger.LogInformation(
            "User with ID `{UserId}` unrestricted successfully by moderator `{Moderator}`.",
            userId,
            currentUserId);
    }

    /// <inheritdoc/>
    public async Task AssignModeratorRoleAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to assign moderator role to user with ID `{UserId}`...", userId);
        
        var currentUserId = GetCurrentUserId();
        
        if (userId == currentUserId)
        {
            logger.LogWarning("Admin with ID `{UserId}` attempted to assign moderator role to themselves.", userId);
            throw new ForbiddenException("Cannot assign moderator role to yourself.");
        }
        
        var user = await GetUserAsync(userId, cancellationToken);
        
         var moderatorRole = await roleRepository
            .AsQueryable()
            .SingleOrDefaultAsync(r => r.Id == (int)Domain.Enums.Role.Moderator, cancellationToken)
            ?? throw new InvalidOperationException($"Role {Domain.Enums.Role.Moderator} does not exist.");
        
        if (user.Roles.Any(r => r.Id == moderatorRole.Id))
        {
            logger.LogWarning("User with ID `{UserId}` already has the moderator role.", userId);
            throw new ConflictException("User is already a moderator.");
        }
        
        user.AddRole(moderatorRole);
        
        await userRepository.UpdateAsync(user, cancellationToken);
        
        logger.LogInformation(
            "Moderator role assigned successfully to user with ID `{UserId}` by admin `{Admin}`.",
            userId,
            currentUserId);
    }

    /// <inheritdoc/>
    public async Task UnassignModeratorRoleAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to remove moderator role from user with ID `{UserId}`...", userId);

        var currentUserId = GetCurrentUserId();
        
        if (userId == currentUserId)
        {
            logger.LogWarning("Admin with ID `{UserId}` attempted to remove moderator role from themselves.", userId);
            throw new ForbiddenException("Cannot remove moderator role from yourself.");
        }
        
        var user = await GetUserAsync(userId, cancellationToken);
        
        var moderatorRole = await roleRepository
            .AsQueryable()
            .SingleOrDefaultAsync(r => r.Id == (int)Domain.Enums.Role.Moderator, cancellationToken)
            ?? throw new InvalidOperationException($"Role {Domain.Enums.Role.Moderator} does not exist.");

        var userModeratorRole = user.Roles.FirstOrDefault(r => r.Id == moderatorRole.Id);
        
        if (userModeratorRole is null)
        {
            logger.LogWarning("User with ID `{UserId}` is not a moderator.", userId);
            throw new ConflictException("User is not a moderator.");
        }
        
        user.RemoveRole(moderatorRole);
        
        await userRepository.UpdateAsync(user, cancellationToken);
        
        logger.LogInformation(
            "Moderator role removed successfully from user with ID `{UserId}` by admin `{Admin}`.",
            userId,
            currentUserId);
    }
    
    private Guid GetCurrentUserId() => authenticationService.GetUserIdFromAccessToken();

    private async Task<(Role restrictedRole, Role userRole)> GetRolesAsync(
        CancellationToken cancellationToken)
    {
        var roles = await roleRepository
            .AsQueryable()
            .Where(r => r.Id == (int)Domain.Enums.Role.Restricted || r.Id == (int)Domain.Enums.Role.User)
            .ToListAsync(cancellationToken);

        var restrictedRole = roles.SingleOrDefault(r => r.Id == (int)Domain.Enums.Role.Restricted)
                             ?? throw new InvalidOperationException($"Role {Domain.Enums.Role.Restricted} does not exist.");
        var userRole = roles.SingleOrDefault(r => r.Id == (int)Domain.Enums.Role.User)
                       ?? throw new InvalidOperationException($"Role {Domain.Enums.Role.User} does not exist.");

        return (restrictedRole, userRole);
    }
    
    private async Task<User> GetUserAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var user = await userRepository
            .AsQueryable()
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
        {
            logger.LogWarning("User with ID `{UserId}` not found.", userId);
            throw new NotFoundException($"User with ID '{userId}' not found.");
        }

        logger.LogInformation("User with ID `{UserId}` retrieved successfully.", userId);
        
        return user;
    }
}