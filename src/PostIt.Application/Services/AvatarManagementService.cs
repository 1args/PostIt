using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostIt.Application.Abstractions.Data;
using PostIt.Application.Abstractions.Services;
using PostIt.Contracts.Exceptions;
using PostIt.Domain.Entities;

namespace PostIt.Application.Services;

/// <inheritdoc/>
public class AvatarManagementService(
    IRepository<User> userRepository,
    IAvatarService avatarService,
    IAuthenticationService authenticationService,
    ILogger<AvatarManagementService> logger) : IAvatarManagementService
{
    /// <inheritdoc/>
    public async Task UploadAvatarAsync(
        ReadOnlyMemory<byte> avatar,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        
        var user = await GetUserOrThrowAsync(userId, cancellationToken);
        
        await using var transaction = await userRepository.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var avatarPath = await avatarService.UploadAvatarAsync(userId, avatar, cancellationToken);

            user.UpdateAvatar(avatarPath);
            await userRepository.UpdateAsync(user, cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ReadOnlyMemory<byte>> GetCurrentUserAvatarAsync(
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var user = await GetUserOrThrowAsync(userId, cancellationToken);

        return await avatarService.DownloadAvatarAsync(user, cancellationToken);
    }
    
    /// <inheritdoc/>
    public async Task<ReadOnlyMemory<byte>> GetAvatarAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var user = await GetUserOrThrowAsync(userId, tracking: false, cancellationToken: cancellationToken);

        return await avatarService.DownloadAvatarAsync(user, cancellationToken);
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