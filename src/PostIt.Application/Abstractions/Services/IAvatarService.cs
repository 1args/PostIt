namespace PostIt.Application.Abstractions.Services;

/// <summary>
/// Provides functionality for uploading user avatars.
/// </summary>
public interface IAvatarService
{
    /// <summary>
    /// Uploads an avatar image for a specified user.
    /// </summary>
    /// <param name="userId">ID of the user to whom the avatar belongs.</param>
    /// <param name="avatar">Binary content of the avatar image.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Path of the uploaded avatar.</returns>
    Task<string> UploadAvatarAsync(Guid userId, ReadOnlyMemory<byte> avatar, CancellationToken cancellationToken);
}