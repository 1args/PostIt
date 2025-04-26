namespace PostIt.Infrastructure.Options;

public class TokenValidationOptions
{
    public bool ValidateIssuerSigningKey { get; set; } = true;
    
    public bool ValidateIssuer { get; set; } = false;
    
    public bool ValidateAudience { get; set; } = false;
    
    public TimeSpan ClockSkew { get; set; } = TimeSpan.FromMinutes(2);
}