namespace PostIt.Infrastructure.Options;

/// <summary>
/// Options for validating JWT tokens.
/// </summary>
public class TokenValidationOptions
{
    /// <summary>Specifies whether to validate the signing key.</summary>
    public bool ValidateIssuerSigningKey { get; set; } = true;
    
    /// <summary>Specifies whether to validate the issuer of the token.</summary>
    public bool ValidateIssuer { get; set; } = false;
    
    /// <summary>Specifies whether to validate the audience of the token.</summary>
    public bool ValidateAudience { get; set; } = false;
    
    /// <summary>Allowed clock skew time to accommodate for time differences between systems.</summary>
    public TimeSpan ClockSkew { get; set; } = TimeSpan.FromMinutes(2);
}