using System.Security.Claims;

namespace PostIt.Application.Abstractions.Security.Authentication;

/// <summary>
/// Provides functionality for JSON Web Tokens.
/// </summary>
public interface IJwtProvider
{
    /// <summary>
    /// Generates a JWT access token based on the specified claims.
    /// </summary>
    /// <param name="claims">Collection of claims to include in the token.</param>
    /// <returns>Signed JWT access token as a string.</returns>
    string GenerateAccessToken(IEnumerable<Claim> claims);

    /// <summary>
    /// Generates a secure refresh token.
    /// </summary>
    /// <returns>A randomly generated refresh token as a string.</returns>
    string GenerateRefreshToken();
    
    /// <summary>
    /// Validates the provided JWT and extracts the claims.
    /// </summary>
    /// <param name="token">JWT token string to validate.</param>
    /// <returns>Collection of claims if the token is valid.</returns>
    IEnumerable<Claim> ValidateToken(string token);
}