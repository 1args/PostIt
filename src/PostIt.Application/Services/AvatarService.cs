using Microsoft.Extensions.Logging;
using PostIt.Application.Abstractions.Data;
using PostIt.Application.Abstractions.Services;

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
        ReadOnlyMemory<byte> avatar, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting avatar upload for user `{UserId}`.", userId);

        var resizedAvatar = await imageProcessor.ResizeImageAsync(avatar, cancellationToken);
        var fileName = $"avatars/{userId}_{DateTime.Now:yyyyMMddHHmmss}.webp";
        
        await fileStorage.UploadFileAsync(fileName, "image/webp", resizedAvatar, cancellationToken);
        
        logger.LogInformation("Avatar uploaded successfully for user `{UserId}` to path `{FileName}`.", userId, fileName);
        
        return fileName;
    }
}