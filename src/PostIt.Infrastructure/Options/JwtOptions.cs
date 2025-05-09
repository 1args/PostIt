namespace PostIt.Infrastructure.Options;

public class JwtOptions
{
    public required string Secret { get; set; }
    
    public int AccessTokenExpirationInHours { get; set; } = 24;

    public int RefreshTokenExpirationInHours { get; set; } = 336; // 14days

    public TokenValidationOptions TokenValidationOptions { get; set; } = new();
}