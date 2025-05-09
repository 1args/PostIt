using System.Security.Claims;
using PostIt.Domain.Entities;

namespace PostIt.Application.Abstractions.Services;

/// <summary>
/// Provides authentication-related functionalities.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Generates an access token for the specified user.
    /// </summary>
    /// <param name="user">User for whom the access token is generated.</param>
    /// <param name="additionalClaims">Optional additional claims to add to the token.</param>
    /// <returns>Generated access token as a string.</returns>
    string GenerateAccessToken(User user, IEnumerable<Claim>? additionalClaims = null);

    /// <summary>
    /// Generates and stores a refresh token for the specified user.
    /// </summary>
    /// <param name="user">User for whom the refresh token is generated.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation with the refresh token as a string.</returns>
    Task<string> GenerateAndStoreRefreshTokenAsync(User user, CancellationToken cancellationToken);

    /// <summary>
    /// Generates both access and refresh tokens for the specified user.
    /// </summary>
    /// <param name="user">User for whom the tokens are generated.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Access and refresh tokens as strings.</returns>
    Task<(string accessToken, string refreshToken)> GenerateAccessAndRefreshTokensAsync(User user,
        CancellationToken cancellationToken);

    /// <summary>
    /// Refreshes the access token using the current refresh token.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Access and refresh tokens as strings.</returns>
    Task<(string accessToken, string refreshToken)> RefreshAccessTokenAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Revokes the current refresh token.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task RevokeRefreshTokenAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves access token from the HTTP request header.
    /// </summary>
    /// <returns>Access token as a string, or null if no token is found.</returns>
    string? GetAccessTokenFromHeader();

    /// <summary>
    /// Retrieves refresh token from the HTTP request header.
    /// </summary>
    /// <returns>Refresh token as a string, or null if no token is found.</returns>
    string? GetRefreshTokenFromHeader();

    /// <summary>
    /// Extracts the user ID from the access token.
    /// </summary>
    /// <returns>User ID extracted from the access.</returns>
    Guid GetUserIdFromAccessToken();
}