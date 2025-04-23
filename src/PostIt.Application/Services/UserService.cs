using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PostIt.Application.Abstractions.Auth;
using PostIt.Application.Abstractions.Data;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Exceptions;
using PostIt.Contracts.ApiContracts.Requests.User;
using PostIt.Contracts.ApiContracts.Responses;
using PostIt.Contracts.Mappers;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects.User;

namespace PostIt.Application.Services;

public class UserService(
    IRepository<User> userRepository,
    IPasswordHasher passwordHasher,
    IAuthenticationService authenticationService,
    ILogger<UserService> logger) : IUserService
{
    public async Task<Guid> RegisterAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating user with email `{Email}`.", request.Email);
        
        var name = Name.Create(request.Name);
        var bio = Bio.Create(request.Bio);
        var email = Email.Create(request.Email);
        
        var isExistingUser = await userRepository.AnyAsync(u => u.Email == email, cancellationToken);

        if (isExistingUser)
        {
            throw new ValidationException($"User with email {request.Email} already exists.");
        }
        
        var passwordHash = passwordHasher.HashPassword(request.Password);
        var user = User.Create(name, bio, email, passwordHash, request.Role, DateTime.UtcNow);
        
        await userRepository.AddAsync(user, cancellationToken);
        
        logger.LogInformation("User created successfully with ID `{UserId}`.", user.Id);
        
        return user.Id;
    }

    public async Task<(string acessToken, string refreshToken)> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Login attempt for `{Email}`...", email);

        var user = await userRepository
            .SingleOrDefaultAsync(u => u.Email.Value == email, cancellationToken, tracking: false);

        if (user is null)
        {
            throw new ValidationException($"User with email {email} does not exist.");
        }

        var result = passwordHasher.VerifyHashedPassword(password, user.PasswordHash);

        if (!result)
        {
            throw new ValidationException("Invalid email or password.");
        }
        
        var (access, refresh) = await authenticationService
            .GenerateAccessAndRefreshTokensAsync(user, cancellationToken);
        
        return (access, refresh);
    }

    public async Task LogoutAsync(HttpRequest request, HttpResponse response, CancellationToken cancellationToken)
    {
        await authenticationService.RevokeRefreshTokenAsync(request, response, cancellationToken);
    }

    public async Task RefreshToken(HttpRequest request, HttpResponse response, CancellationToken cancellationToken)
    {
        var refreshToken = authenticationService.GetRefreshTokenFromHeader(request)
                          ?? throw new ValidationException("Refresh token is missing.");

        var (userId, _) = authenticationService.GetUserDataFromToken(refreshToken);
        
        var user = await GetUserOrThrowAsync(userId, cancellationToken, tracking: false);
        
        await authenticationService.RefreshAccessTokenAsync(request, response, user, cancellationToken);
    }
    
    public async Task<UserResponse> GetUserByIdAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching user by ID `{UserId}`.", userId);

        var user = await GetUserOrThrowAsync(
            userId,
            cancellationToken,
            tracking: false);
        
        logger.LogInformation("User with ID `{UserId}` retrieved successfully.", userId);
        
        return user.MapToPublic();
    }

    public async Task DeleteUserAsync(
        Guid userId, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting user with ID `{UserId}`.", userId);
        
        var user = await GetUserOrThrowAsync(userId, cancellationToken);

        await userRepository.DeleteAsync([user], cancellationToken);
        logger.LogInformation("User with ID `{UserId}` deleted successfully.", userId);
    }

    public async Task UpdateUserBioAsync(
        Guid userId,
        UpdateUserBioRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating bio for user with ID `{UserId}`.", userId);
        
        var user = await GetUserOrThrowAsync(userId, cancellationToken);

        var newBio = Bio.Create(request.Bio);
        user.UpdateBio(newBio);
        
        await userRepository.UpdateAsync(user, cancellationToken);
        
        logger.LogInformation("Bio updated successfully for user with ID `{UserId}`.",
            userId);
    }

    private async Task<User> GetUserOrThrowAsync(
        Guid userId,
        CancellationToken cancellationToken,
        bool tracking = true,
        params Expression<Func<User, object>>[] includes)
    {
        var user = await userRepository
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken, tracking: tracking, includes: includes);

        if (user is null)
        {
            logger.LogWarning("User with ID `{UserId}` not found.", userId);
            throw new NotFoundException($"User with ID '{userId}' not found.");
        }

        logger.LogInformation("User with ID `{UserId}` retrieved successfully.", userId);
        
        return user;
    }
}