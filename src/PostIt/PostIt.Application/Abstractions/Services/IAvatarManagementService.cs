namespace PostIt.Application.Abstractions.Services;

/// <summary>
/// Provides functionality for managing user avatars.
/// </summary>
public interface IAvatarManagementService
{
    /// <summary>
    /// Uploads a new avatar image for the currently authenticated user.
    /// </summary>
    /// <param name="avatar">Avatar image to upload as a byte array.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UploadAvatarAsync(ReadOnlyMemory<byte> avatar, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the avatar image for the currently authenticated user.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Avatar in byte.</returns>
    Task<ReadOnlyMemory<byte>> GetCurrentUserAvatarAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Gets the avatar image for the user by their ID.
    /// </summary>
    /// <param name="userId">ID of the user whose avatar be retrieved.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Avatar in byte.</returns>
    Task<ReadOnlyMemory<byte>> GetAvatarAsync(Guid userId, CancellationToken cancellationToken);
}