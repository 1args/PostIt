namespace PostIt.Infrastructure.Options.Jwt;

public class JwtOptions
{
    public required string AccessTokenSecret { get; set; }

    public required string RefreshTokenSecret { get; set; }
    
    public int AccessTokenExpirationInHours { get; set; } = 24;

    public int RefreshTokenExpirationInHours { get; set; } = 336; // 14days

    public TokenValidationOptions TokenValidationOptions { get; set; } = new();
}