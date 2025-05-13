using PostIt.Contracts.ApiContracts.Requests.User;
using PostIt.Contracts.ApiContracts.Responses;

namespace PostIt.Application.Abstractions.Services;

/// <summary>
/// Provides functionality for managing user accounts.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="request">Request containing the user's information to register.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>ID of the registered user.</returns>
    Task<Guid> RegisterAsync(CreateUserRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="request">Request containing the login credentials.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Login response containing authentication details.</returns>
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Verifies the user's email using a verification token.
    /// </summary>
    /// <param name="userId">ID of the user.</param>
    /// <param name="token">Token used to verify the email.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Indicating whether the email verification was successful.</returns>
    Task<bool> VerifyEmailAsync(Guid userId, Guid token, CancellationToken cancellationToken);

    /// <summary>
    /// Logs out the currently authenticated user.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task LogoutAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Refreshes the user's authentication token.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Login response containing the refreshed authentication details.</returns>
    Task<AuthResponse> RefreshToken(CancellationToken cancellationToken);

    /// <summary>
    /// Uploads a new avatar image for the currently authenticated user.
    /// </summary>
    /// <param name="avatar">Avatar image to upload as a byte array.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UploadAvatarAsync(ReadOnlyMemory<byte> avatar, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the avatar image for the currently authenticated user.
    /// </summary>
    /// <param name="userId">ID of the user whose avatar be retrieved.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Avatar in byte.</returns>
    Task<ReadOnlyMemory<byte>> GetAvatarAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves the user details.
    /// </summary>
    /// <param name="userId">ID of the user whose details are to be retrieved.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Response containing the user's details.</returns>
    Task<UserResponse> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a user account by its ID.
    /// </summary>
    /// <param name="userId">ID of the user to be deleted.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the bio information of a user.
    /// </summary>
    /// <param name="userId">ID of the user whose bio will be updated.</param>
    /// <param name="request">Request containing the new bio information.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UpdateUserBioAsync(Guid userId, UpdateUserBioRequest request, CancellationToken cancellationToken);
}