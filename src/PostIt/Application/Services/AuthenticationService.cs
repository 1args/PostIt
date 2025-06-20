using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PostIt.Application.Abstractions.Data;
using PostIt.Application.Abstractions.Security.Authentication;
using PostIt.Application.Abstractions.Services;
using PostIt.Contracts.Exceptions;
using PostIt.Domain.Entities;

namespace PostIt.Application.Services;

/// <inheritdoc/>
public class AuthenticationService(
    IRepository<User> userRepository,
    IJwtProvider jwtProvider,
    ITokenStorage tokenStorage,
    IHttpContextAccessor httpContextAccessor) : IAuthenticationService
{
    private const string AuthorizationHeader = "Authorization";
    private const string RefreshTokenHeader = "Refresh-Token";
    private const string BearerPrefix = "Bearer";
    
    /// <inheritdoc/>
    public string GenerateAccessToken(User user, IEnumerable<Claim>? additionalClaims = null)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        }; 

        if (additionalClaims is not null)
        {
            claims.AddRange(additionalClaims);
        }        
        
        return jwtProvider.GenerateAccessToken(claims);
    }

    /// <inheritdoc/>
    public async Task<string> GenerateAndStoreRefreshTokenAsync(User user, CancellationToken cancellationToken)
    {
        var refreshToken = jwtProvider.GenerateRefreshToken();
        await tokenStorage.SetTokenAsync(refreshToken, user.Id, cancellationToken);
        
        return refreshToken;
    }

    /// <inheritdoc/>
    public async Task<(string accessToken, string refreshToken)> GenerateAccessAndRefreshTokensAsync(
        User user, 
        CancellationToken cancellationToken)
    {
        var accessToken = GenerateAccessToken(user);
        var refreshToken = await GenerateAndStoreRefreshTokenAsync(user, cancellationToken);
        
        return (accessToken, refreshToken);
    }
    
    /// <inheritdoc/>
    public async Task<(string accessToken, string refreshToken)> RefreshAccessTokenAsync(CancellationToken cancellationToken)
    {
        var refreshToken = GetRefreshTokenFromHeader();

        if (refreshToken is null)
        {
            throw new UnauthorizedException("Refresh token is missing.");
        }
        
        var (_, userId) = await tokenStorage.GetTokenAsync(refreshToken, cancellationToken);
        
        var user = await userRepository
            .AsQueryable()
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException("User not found.");
        }

        var accessToken = GenerateAccessToken(user);
        return (accessToken, refreshToken);
    }
    
    /// <inheritdoc/>
    public async Task RevokeRefreshTokenAsync(CancellationToken cancellationToken)
    {
        var request = httpContextAccessor.HttpContext?.Request;
        var response = httpContextAccessor.HttpContext?.Response;

        response?.Headers.Remove(AuthorizationHeader);
        
        if (request.Headers.TryGetValue(RefreshTokenHeader, out var refreshToken)
            && !string.IsNullOrWhiteSpace(refreshToken))
        {
            response?.Headers.Remove(RefreshTokenHeader);
            
            await tokenStorage.RemoveTokenAsync(refreshToken, cancellationToken);
        }
    }
    
    /// <inheritdoc/>
    public string? GetAccessTokenFromHeader() 
    {
        var header = httpContextAccessor.HttpContext?.Request.Headers[AuthorizationHeader];

        if (header.HasValue && !string.IsNullOrWhiteSpace(header.Value))
        {
            var token = header.Value.ToString();
        
            if (token.StartsWith(BearerPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return token[BearerPrefix.Length..].Trim();
            }
        }

        return null;
    }
    
    /// <inheritdoc/>
    public string? GetRefreshTokenFromHeader()
    {
        var header = httpContextAccessor.HttpContext?.Request.Headers[RefreshTokenHeader];
        return header.ToString();
    }
    
    /// <inheritdoc/>
    public Guid GetUserIdFromAccessToken()
    {
        var accessToken = GetAccessTokenFromHeader();

        if (accessToken is null)
        {
            throw new UnauthorizedException("Access token is missing.");
        };
        
        var claims = jwtProvider.ValidateToken(accessToken).ToArray();
        return GetUserIdFromClaims(claims);
    }
    
    private static Guid GetUserIdFromClaims(Claim[] claims)
    {
        var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            throw new ReadableException("Invalid user identifier in token.");
        }
        
        return userId;
    }
}