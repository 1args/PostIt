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
    public async Task<UserResponse> GetCurrentUserAsync(
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        logger.LogInformation("Fetching user by ID `{UserId}`.", userId);

       var user = await GetUserOrThrowAsync(userId, tracking: false, cancellationToken: cancellationToken);

       return user.MapToPublic();
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
        CancellationToken cancellationToken,
        bool tracking = true)
    {
        var query = userRepository.AsQueryable();
        
        query = tracking ? query.AsTracking() : query.AsNoTracking();
        
        var user = await query
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