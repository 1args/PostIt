namespace PostIt.Infrastructure.Options;

/// <summary>
/// Configuration options for JWT authentication.
/// </summary>
public class JwtOptions
{
    /// <summary>Secret key used for signing JWT tokens.</summary>
    public required string Secret { get; set; }
    
    /// <summary>Access token expiration time in hours.</summary>
    public int AccessTokenExpirationInHours { get; set; } = 24;

    /// <summary>Refresh token expiration time in hours.</summary>
    public int RefreshTokenExpirationInHours { get; set; } = 336; // 14days

    /// <summary>Options for validating JWT tokens.</summary>
    public TokenValidationOptions TokenValidationOptions { get; set; } = new();
}