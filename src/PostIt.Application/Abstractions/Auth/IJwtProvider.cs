using System.Security.Claims;

namespace PostIt.Application.Abstractions.Auth;

public interface IJwtProvider
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    
    IEnumerable<Claim> ValidateAccessToken(string token);
    
    string GenerateRefreshToken();
}