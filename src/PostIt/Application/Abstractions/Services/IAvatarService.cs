using PostIt.Domain.Entities;

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
    /// <param name="payload">Image data as a stream.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Path of the uploaded avatar.</returns>
    Task<string> UploadAvatarAsync(Guid userId, Stream payload, CancellationToken cancellationToken);

    /// <summary>
    /// Downloads the avatar image of a given user.
    /// </summary>
    /// <param name="user">User entity whose avatar is to be downloaded.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Stream containing the user's avatar image.</returns>
    Task<Stream> DownloadAvatarAsync(User user, CancellationToken cancellationToken);
}