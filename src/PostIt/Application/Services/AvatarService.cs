using Microsoft.Extensions.Logging;
using PostIt.Application.Abstractions.Data;
using PostIt.Application.Abstractions.Services;
using PostIt.Contracts.Exceptions;
using PostIt.Domain.Entities;

namespace PostIt.Application.Services;

/// <inheritdoc/>
public class AvatarService(
    IMinioFileStorage fileStorage, 
    IImageProcessor imageProcessor,
    ILogger<AvatarService> logger) : IAvatarService
{
    /// <inheritdoc/>
    public async Task<string> UploadAvatarAsync(
        Guid userId,
        Stream payload, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting avatar upload for user `{UserId}`.", userId);

        var resizedAvatar = await imageProcessor.ResizeImageAsync(payload, cancellationToken);
        var fileName = $"avatars/{userId}_{DateTime.Now:yyyyMMddHHmmss}.webp";
        
        await fileStorage.UploadFileAsync(fileName, "image/webp", resizedAvatar, cancellationToken);
        
        logger.LogInformation("Avatar uploaded successfully for user `{UserId}` to path `{FileName}`.", userId, fileName);
        
        return fileName;
    }

    public async Task<Stream> DownloadAvatarAsync(
        User user, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting avatar download for user `{UserId}`.", user.Id);

        if (user.Avatar is null)
        {
            throw new NotFoundException($"User with ID {user.Id} has not yet uploaded an avatar.");
        }
        
        var avatar = await fileStorage.DownloadFileAsync(user.Avatar, cancellationToken);
        
        logger.LogInformation("Avatar downloaded successfully for user `{UserId}`.", user.Id);
        
        return avatar;
    }
}