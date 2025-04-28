using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PostIt.Domain.Entities;

namespace PostIt.Application.Abstractions.Services;

public interface IAuthenticationService
{
    string GenerateAccessToken(User user, IEnumerable<Claim>? additionalClaims = null);

    Task<string> GenerateAndStoreRefreshTokenAsync(User user, CancellationToken cancellationToken);

    Task<(string accessToken, string refreshToken)> GenerateAccessAndRefreshTokensAsync(User user,
        CancellationToken cancellationToken);
    
    ClaimsPrincipal? ValidateToken(string token);

    ValueTask<string?> GetAccessTokenFromHeader(HttpRequest request);

    string? GetRefreshTokenFromHeader(HttpRequest request);

    void SetTokensToResponse(HttpResponse response, string accessToken, string refreshToken);

    Task RefreshAccessTokenAsync(HttpRequest request, HttpResponse response, User user, 
        CancellationToken cancellationToken);
    
    Task RevokeRefreshTokenAsync(HttpRequest request, HttpResponse response, CancellationToken cancellationToken);

    (Guid userId, string role) GetUserDataFromToken(string token);
}