using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostIt.Application.Abstractions.Data;
using PostIt.Application.Abstractions.Services;
using PostIt.Common.Abstractions;
using PostIt.Contracts.Exceptions;
using PostIt.Contracts.Mappers;
using PostIt.Contracts.Requests.User;
using PostIt.Contracts.Responses;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects;

namespace PostIt.Application.Services;

/// <inheritdoc/>
public class UserService(
    IRepository<User> userRepository,
    IAuthenticationService authenticationService,
    ITransactionManager transactionManager,
    ILogger<UserService> logger) : IUserService
{
    public async Task<UserResponse> GetCurrentUserAsync(
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        logger.LogInformation("Fetching user by ID `{UserId}`.", userId);

       var user = await GetUserAsync(
           userId,
           tracking: false,
           includes: true, 
           cancellationToken: cancellationToken);

       return user.MapToPublic();
    }
    
    /// <inheritdoc/>
    public async Task<UserResponse> GetUserByIdAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching user by ID `{UserId}`.", userId);

        var user = await GetUserAsync(
            userId, 
            tracking: false, 
            includes: true, 
            cancellationToken: cancellationToken);
        
        logger.LogInformation("User with ID `{UserId}` retrieved successfully.", userId);
        
        return user.MapToPublic();
    }

    /// <inheritdoc/>
    public async Task DeleteUserAsync(
        Guid userId, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting user with ID `{UserId}`.", userId);
        
        var user = await GetUserAsync(userId, cancellationToken);

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
        
        var user = await GetUserAsync(userId, cancellationToken);

        var newBio = UserBio.Create(request.Bio);
        user.UpdateBio(newBio);
        
        await userRepository.UpdateAsync(user, cancellationToken);
        
        logger.LogInformation("Bio updated successfully for user with ID `{UserId}`.",
            userId);
    }

    /// <inheritdoc/>
    public async Task FollowUserAsync(
        Guid followingId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        
        logger.LogInformation(
            "User `{UserId}` is attempting to follow user `{FollowingId}`...",
            userId,
            followingId);
        
        if (userId == followingId)
        {
            logger.LogWarning("User `{UserId}` attempted to follow themselves.", userId);
            throw new ConflictException("A user cannot follow themselves.");
        }
        
        var follower = await GetUserAsync(userId, cancellationToken);
        var following = await GetUserAsync(followingId, cancellationToken);

        var isAlreadyFollowing = await userRepository
            .AsQueryable()
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Followings)
            .AnyAsync(f => f.Id == followingId, cancellationToken);
        
        if (isAlreadyFollowing)
        {
            logger.LogWarning(
                "User `{UserId}` is already following user `{FollowingId}`.",
                userId,
                followingId);
            throw new ConflictException($"User is already following user with ID '{followingId}'.");
        }
        
        follower.AddFollowing(following);
        following.AddFollower(follower);

        await transactionManager.ExecuteInTransactionAsync(async () =>
        {
            await userRepository.UpdateRangeAsync([follower, following], cancellationToken);
        }, cancellationToken);
        
        logger.LogInformation(
            "User {UserId} successfully followed user {FollowingId}.", 
            userId,
            followingId);
    }

    /// <inheritdoc/>
    public async Task UnfollowUserAsync(
        Guid followingId, 
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        
        logger.LogInformation(
            "User `{UserId}` is attempting to unfollow user `{FollowingId}`...",
            userId,
            followingId);
        
        if (userId == followingId)
        {
            logger.LogWarning("User `{UserId}` attempted to unfollow themselves.", userId);
            throw new ConflictException("A user cannot unfollow themselves.");
        }
        
        var follower = await GetUserAsync(userId, cancellationToken);
        var following = await GetUserAsync(followingId, cancellationToken);

        var isFollowing = await userRepository
            .AsQueryable()
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Followings)
            .AnyAsync(f => f.Id == followingId, cancellationToken);
        
        if (!isFollowing)
        {
            logger.LogWarning("User `{UserId}` is not following user `{FollowingId}`.", userId, followingId);
            throw new BadRequestException($"Ви не стежите за користувачем з ID '{followingId}'.");
        }
        
        follower.RemoveFollowing(following);
        following.RemoveFollower(follower);
        
        await transactionManager.ExecuteInTransactionAsync(async () =>
        {
            await userRepository.UpdateRangeAsync([follower, following], cancellationToken);
        }, cancellationToken);
   
        logger.LogInformation(
            "User {UserId} successfully unfollowed user {FollowingId}.", 
            userId,
            followingId);
    }

    private Guid GetCurrentUserId() => authenticationService.GetUserIdFromAccessToken();
    
    private async Task<User> GetUserAsync(
        Guid userId,
        CancellationToken cancellationToken,
        bool tracking = true,
        bool includes = false)
    {
        var query = userRepository.AsQueryable();
        query = tracking ? query.AsTracking() : query.AsNoTracking();
        
        if (includes)
        {
            query = query
                .Include(u => u.Posts)
                .Include(u => u.Comments);
        }
        
        var user = await query.SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
        {
            logger.LogWarning("User with ID `{UserId}` not found.", userId);
            throw new NotFoundException($"User with ID '{userId}' not found.");
        }

        logger.LogInformation("User with ID `{UserId}` retrieved successfully.", userId);
        return user;
    }
}