using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostIt.Application.Abstractions.Authentication;
using PostIt.Application.Abstractions.Data;
using PostIt.Application.Abstractions.Services;
using PostIt.Contracts.ApiContracts.Requests.User;
using PostIt.Contracts.ApiContracts.Responses;
using PostIt.Contracts.Exceptions;
using PostIt.Contracts.Mappers;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects;

namespace PostIt.Application.Services;

/// <inheritdoc/>
public class UserService(
    IRepository<User> userRepository,
    IPasswordHasher passwordHasher,
    IAuthenticationService authenticationService,
    IEmailVerificationService emailVerificationService,
    IAvatarService avatarService,
    ILogger<UserService> logger) : IUserService
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
            .AnyAsync(u => u.Email.Value == email.Value, cancellationToken);

        if (userExists)
        {
            throw new ConflictException($"User with email {request.Email} already exists.");
        }
        
        var passwordHash = passwordHasher.HashPassword(request.Password);
        var password = UserPassword.Create(passwordHash);
        var user = User.Create(name, bio, email, password, request.Role, DateTime.UtcNow);
        
        await userRepository.AddAsync(user, cancellationToken);

        await emailVerificationService.SendVerificationEmailAsync(user, cancellationToken);
        
        logger.LogInformation("User created successfully with ID `{UserId}`.", user.Id);
        
        return user.Id;
    }

    /// <inheritdoc/>
    public async Task<bool> VerifyEmailAsync(
        Guid userId,
        Guid token,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Verifying email for user with ID `{UserId}` and token `{Token}`...", userId, token);

        var user = await GetUserOrThrowAsync(userId, cancellationToken);

        if (user.IsEmailConfirmed)
        {
            logger.LogInformation("Email already confirmed for user {UserId}", userId);
            return false;
        }

        var isVerified = await emailVerificationService.VerifyEmailAsync(user: user, token, cancellationToken);

        if (isVerified)
        {
            user.ConfirmEmail();
            await userRepository.UpdateAsync(user, cancellationToken);
            logger.LogInformation("Email verified successfully for user with ID `{UserId}`.", userId);
        }
 
        return isVerified;
    }

    /// <inheritdoc/>
    public async Task<AuthResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Login attempt for `{Email}`...", request.Email);

        var user = await userRepository
            .AsQueryable()
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email.Value == request.Email, cancellationToken);

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
    public async Task UploadAvatarAsync(
        ReadOnlyMemory<byte> avatar,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        
        var user = await GetUserOrThrowAsync(userId, cancellationToken);
        
        var avatarPath = await avatarService.UploadAvatarAsync(userId, avatar, cancellationToken);
        
        user.UpdateAvatar(avatarPath);
        await userRepository.UpdateAsync(user, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ReadOnlyMemory<byte>> GetAvatarAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var user = await GetUserOrThrowAsync(userId, cancellationToken);

        return await avatarService.DownloadAvatarAsync(user, cancellationToken);
    }
    
    /// <inheritdoc/>
    public async Task<UserResponse> GetUserByIdAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching user by ID `{UserId}`.", userId);

        var user = await GetUserOrThrowAsync(userId, cancellationToken);
        
        logger.LogInformation("User with ID `{UserId}` retrieved successfully.", userId);
        
        return user.MapToPublic();
    }

    /// <inheritdoc/>
    public async Task DeleteUserAsync(
        Guid userId, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting user with ID `{UserId}`.", userId);
        
        var user = await GetUserOrThrowAsync(userId, cancellationToken);

        await userRepository.DeleteAsync([user], cancellationToken);
        logger.LogInformation("User with ID `{UserId}` deleted successfully.", userId);
    }

    /// <inheritdoc/>
    public async Task UpdateUserBioAsync(
        Guid userId,
        UpdateUserBioRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating bio for user with ID `{UserId}`.", userId);
        
        var user = await GetUserOrThrowAsync(userId, cancellationToken);

        var newBio = UserBio.Create(request.Bio);
        user.UpdateBio(newBio);
        
        await userRepository.UpdateAsync(user, cancellationToken);
        
        logger.LogInformation("Bio updated successfully for user with ID `{UserId}`.",
            userId);
    }
    
    private Guid GetCurrentUserId() => authenticationService.GetUserIdFromAccessToken();

    private async Task<User> GetUserOrThrowAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var user = await userRepository
            .AsQueryable()
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