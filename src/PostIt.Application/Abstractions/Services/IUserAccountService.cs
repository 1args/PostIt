using PostIt.Contracts.ApiContracts.Requests.User;
using PostIt.Contracts.ApiContracts.Responses;

namespace PostIt.Application.Abstractions.Services;

/// <summary>
/// Provides functionality for the user account in terms of authentication.
/// </summary>
public interface IUserAccountService
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
}