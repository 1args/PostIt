using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PostIt.Application.Abstractions.Auth;
using PostIt.Application.Abstractions.Data;
using PostIt.Application.Abstractions.Services;
using PostIt.Domain.Entities;
using PostIt.Domain.Enums;

namespace PostIt.Application.Services;

public class AuthenticationService(
    IJwtProvider jwtProvider,
    ICacheService cacheService) : IAuthenticationService
{
    private const string RefreshPrefix = "refresh";
    private const string AuthorizationHeader = "Authorization";
    private const string RefreshTokenHeader = "Refresh-Token";
    private const string BearerPrefix = "Bearer";
    
    public string GenerateAccessToken(User user, IEnumerable<Claim>? additionalClaims = null)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email.ToString()),
            new(ClaimTypes.Role, user.Role.ToString())
        }; 

        if (additionalClaims is not null)
        {
            claims.AddRange(additionalClaims);
        }        
        
        return jwtProvider.GenerateAccessToken(claims);
    }

    public async Task<string> GenerateAndStoreRefreshTokenAsync(User user, CancellationToken cancellationToken)
    {
        var (refreshToken, expiresIn) = jwtProvider.GenerateRefreshToken();
        
        var expiration = DateTime.UtcNow.Add(expiresIn);
        
        var key = GetRefreshKey(user.Id, refreshToken);
        
        await cacheService.SetAsync(key, refreshToken, cancellationToken);
        
        return refreshToken;
    }

    public async Task<(string accessToken, string refreshToken)> GenerateAccessAndRefreshTokensAsync(
        User user, 
        CancellationToken cancellationToken)
    {
        var accessToken = GenerateAccessToken(user);
        var refreshToken = await GenerateAndStoreRefreshTokenAsync(user, cancellationToken);
        
        return (accessToken, refreshToken);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        return jwtProvider.ValidateToken(token) is IEnumerable<Claim> claims 
            ? new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"))
            : null;
    }

    public string? GetAccessTokenFromHeader(HttpRequest request)
    {
        if (!request.Headers.TryGetValue(AuthorizationHeader, out var authorizationHeaderValue))
        {
            return null;
        }

        var token = authorizationHeaderValue.ToString();
        return token.StartsWith(BearerPrefix) ? token[BearerPrefix.Length..] : null;
    }
    
    public string? GetRefreshTokenFromHeader(HttpRequest request)
    {
        if (!request.Headers.TryGetValue(RefreshTokenHeader, out var refreshTokenHeaderValue))
        {
            return null;
        }

        return refreshTokenHeaderValue.ToString();
    }

    public void SetTokensToResponse(HttpResponse response, string accessToken, string refreshToken)
    {
        response.Headers.Append(AuthorizationHeader, $"{BearerPrefix}{accessToken}");
        response.Headers.Append(RefreshTokenHeader, refreshToken);
    }
    
    public async Task RefreshAccessTokenAsync(
        HttpRequest request, 
        HttpResponse response, 
        User user,
        CancellationToken cancellationToken)
    {
        var oldRefresh = GetRefreshTokenFromHeader(request) 
                         ?? throw new ArgumentException("Refresh token is missing.");
        
        var oldAccess = GetAccessTokenFromHeader(request)
                        ?? throw new ArgumentException("Access token is missing.");
        
        var claimsPrincipal = ValidateToken(oldRefresh);
        
        if (claimsPrincipal is null)
        {
            throw new ArgumentException("Invalid refresh token");
        }

        var (userId, _) = GetUserDataFromToken(oldRefresh);
        var key = GetRefreshKey(userId, oldRefresh);
        
        var cachedToken = await cacheService.GetAsync<string>(key, cancellationToken);

        if (cachedToken != oldRefresh)
        {
            throw new ArgumentException("Refresh token not found or expired.");
        }
        
        await cacheService.RemoveAsync(key, cancellationToken);
        
        var newAccess  = GenerateAccessToken(user);
        var newRefresh = await GenerateAndStoreRefreshTokenAsync(user, cancellationToken);

        SetTokensToResponse(response, newAccess, newRefresh);
    }

    public async Task RevokeRefreshTokenAsync(HttpRequest request, HttpResponse response, CancellationToken cancellationToken)
    {
        response.Headers.Remove(AuthorizationHeader);
        
        var refreshToken = GetRefreshTokenFromHeader(request) ??
                           throw new ArgumentException("Refresh token is missing.");;
        
        var (userId, _) = GetUserDataFromToken(refreshToken);
        var key = GetRefreshKey(userId, refreshToken);
        
        await cacheService.RemoveAsync(key, cancellationToken);
    }
    
    public (Guid userId, string role) GetUserDataFromToken(string token)
    {
        var claims = jwtProvider.ValidateToken(token).ToArray();
        
        var userIdClaims = claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var roleClaims = claims.First(c => c.Type == ClaimTypes.Role).Value;
        
        return (Guid.Parse(userIdClaims), roleClaims);
    }

    private string GetRefreshKey(Guid userId, string refreshToken) => $"{RefreshPrefix}:{userId}:{refreshToken}";
}