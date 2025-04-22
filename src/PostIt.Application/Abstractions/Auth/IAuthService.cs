using PostIt.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace PostIt.Application.Abstractions.Auth;

public interface IAuthService
{
    string GenerateAccessToken(User user);
    
    Task<string> GenerateRefreshTokenAsync(User user, CancellationToken cancellationToken);
    
    Task<(string accessToken, string refreshToken)> GenerateAccessAndRefreshTokensAsync(User user, CancellationToken cancellationToken);
    
    void ValidateToken(string token);
    
    Task RefreshAccessTokenAsync(HttpRequest request, HttpResponse response, CancellationToken cancellationToken);
}