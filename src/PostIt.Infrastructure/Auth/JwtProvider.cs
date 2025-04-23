using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PostIt.Application.Abstractions.Auth;
using PostIt.Infrastructure.Options.Jwt;

namespace PostIt.Infrastructure.Auth;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;
    
    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var securityTokenDescriptor = GetSecurityTokenDescriptor(
            _options.AccessTokenSecret,
            _options.AccessTokenExpirationInHours,
            claims.ToArray());

        var accessToken = tokenHandler.CreateToken(securityTokenDescriptor);
        
        return tokenHandler.WriteToken(accessToken);
    }

    public (string token, TimeSpan expiresIn) GenerateRefreshToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var securityTokenDescriptor = GetSecurityTokenDescriptor(
            _options.RefreshTokenSecret,
            _options.RefreshTokenExpirationInHours);

        var refreshToken = tokenHandler.CreateToken(securityTokenDescriptor);
        var expiresIn = TimeSpan.FromHours(_options.RefreshTokenExpirationInHours);
        
        return (tokenHandler.WriteToken(refreshToken), expiresIn);
    }
    
    public IEnumerable<Claim> ValidateToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Token cannot be empty.");
        }
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationOptions = _options.TokenValidationOptions;

        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = tokenValidationOptions.ValidateIssuerSigningKey,
            ValidateIssuer = tokenValidationOptions.ValidateIssuer,
            ValidateAudience = tokenValidationOptions.ValidateAudience,
            ClockSkew = tokenValidationOptions.ClockSkew
        };

        var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);

        return claimsPrincipal.Claims;
    }

    private static SecurityTokenDescriptor GetSecurityTokenDescriptor(
        string secretKey,
        int expirationHours, 
        params Claim[]? claims)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            SecurityAlgorithms.HmacSha256Signature);
        
        return new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(expirationHours),
            SigningCredentials = signingCredentials
        };
    }
}