namespace PostIt.Application.Abstractions.Services;

/// <summary>
/// Provides functionality for managing user avatars.
/// </summary>
public interface IAvatarManagementService
{
    /// <summary>
    /// Uploads a new avatar image for the currently authenticated user.
    /// </summary>
    /// <param name="payload">Image data to upload, provided as a stream.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UploadAvatarAsync(Stream payload, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the avatar image for the currently authenticated user.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Stream containing the current user's avatar image.</returns>
    Task<Stream> GetCurrentUserAvatarAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Gets the avatar image for the user by their ID.
    /// </summary>
    /// <param name="userId">ID of the user whose avatar be retrieved.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Stream containing the avatar image of the specified user.</returns>
    Task<Stream> GetAvatarAsync(Guid userId, CancellationToken cancellationToken);
}