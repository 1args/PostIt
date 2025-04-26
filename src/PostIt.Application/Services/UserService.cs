using System.Linq.Expressions;
using Hangfire;
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
    IBackgroundJobClient backgroundJobClient,
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
        
        var isExistingUser = await userRepository.AnyAsync(u => u.Email.Value == email.Value, cancellationToken);

        if (isExistingUser)
        {
            throw new ConflictException($"User with email {request.Email} already exists.");
        }
        
        var passwordHash = passwordHasher.HashPassword(request.Password);
        var password = Password.Create(passwordHash);
        var user = User.Create(name, bio, email, password, request.Role, DateTime.UtcNow);
        
        await userRepository.AddAsync(user, cancellationToken);

        // Test version
        backgroundJobClient.Enqueue<IEmailService>(emailService => 
            emailService.SendEmailAsync(user.Email.Value, "Welcome", $"User {user.Name} Created!"));
        
        logger.LogInformation("User created successfully with ID `{UserId}`.", user.Id);
        
        return user.Id;
    }

    public async Task<LoginResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Login attempt for `{Email}`...", request.Email);

        var user = await userRepository
            .SingleOrDefaultAsync(u => u.Email.Value == request.Email, cancellationToken, tracking: false);

        if (user is null)
        {
            throw new UnauthorizedException($"User with email {request.Email} does not exist.");
        }

        var verifiedPasswordHash = passwordHasher.VerifyHashedPassword(request.Password, user.Password.Value);

        if (!verifiedPasswordHash)
        {
            throw new UnauthorizedException("Invalid email or password.");
        }
        
        var (accessToken, refreshToken) = await authenticationService
            .GenerateAccessAndRefreshTokensAsync(user, cancellationToken);
        
        return new LoginResponse(
            user.Id,
            accessToken,
            refreshToken);
    }

    public async Task LogoutAsync(HttpRequest request, HttpResponse response, CancellationToken cancellationToken)
    {
        await authenticationService.RevokeRefreshTokenAsync(request, response, cancellationToken);
    }

    public async Task RefreshToken(HttpRequest request, HttpResponse response, CancellationToken cancellationToken)
    {
        var refreshToken = authenticationService.GetRefreshTokenFromHeader(request)
                          ?? throw new UnauthorizedException("Refresh token is missing.");

        var (userId, _) = authenticationService.GetUserDataFromToken(refreshToken);
        
        var user = await GetUserOrThrowAsync(userId, cancellationToken);
        
        await authenticationService.RefreshAccessTokenAsync(request, response, user, cancellationToken);
    }
    
    public async Task<UserResponse> GetUserByIdAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching user by ID `{UserId}`.", userId);

        var user = await GetUserOrThrowAsync(userId, cancellationToken);
        
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
        CancellationToken cancellationToken)
    {
        var user = await userRepository
            .GetByIdAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
        {
            logger.LogWarning("User with ID `{UserId}` not found.", userId);
            throw new NotFoundException($"User with ID '{userId}' not found.");
        }

        logger.LogInformation("User with ID `{UserId}` retrieved successfully.", userId);
        
        return user;
    }
}