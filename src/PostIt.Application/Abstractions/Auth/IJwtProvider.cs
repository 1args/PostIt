using System.Security.Claims;

namespace PostIt.Application.Abstractions.Auth;

public interface IJwtProvider
{
    string GenerateAccessToken(IEnumerable<Claim> claims);

    (string token, TimeSpan expiresIn) GenerateRefreshToken();
    
    IEnumerable<Claim> ValidateToken(string token);
}