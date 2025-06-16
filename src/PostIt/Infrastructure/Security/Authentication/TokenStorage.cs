using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using PostIt.Application.Abstractions.Security.Authentication;
using PostIt.Contracts.Exceptions;
using PostIt.Contracts.Options;

namespace PostIt.Infrastructure.Security.Authentication; 

/// <inheritdoc/>
public class TokenStorage(
    IDistributedCache redisCache,
    IOptions<JwtOptions> jwtOptions) : ITokenStorage
{
    private const string TokenFormat = "refresh-token:{0}";

    private readonly TimeSpan _tokenLifetime = TimeSpan.FromHours(jwtOptions.Value.RefreshTokenExpirationInHours);
    
    /// <inheritdoc/>
    public async Task<(string token, Guid userId)> GetTokenAsync(string token, CancellationToken cancellationToken)
    {
        var tokenKey = string.Format(TokenFormat, token);
        var storedGuid = await redisCache.GetStringAsync(tokenKey, cancellationToken);

        if (storedGuid is null)
        {
            throw new UnauthorizedException("The token has expired.");
        }
        if (!Guid.TryParse(storedGuid, out var userId))
        {
            throw new UnauthorizedException("Invalid token format.");
        }

        return (token, userId);
    }

    /// <inheritdoc/>
    public async Task SetTokenAsync(string token, Guid userId, CancellationToken cancellationToken)
    {
        var tokenKey = string.Format(TokenFormat, token);

        await redisCache.SetStringAsync(tokenKey, userId.ToString(), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _tokenLifetime
        }, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task RemoveTokenAsync(string token, CancellationToken cancellationToken)
    {
        var tokenKey = string.Format(TokenFormat, token);
        await redisCache.RemoveAsync(tokenKey, cancellationToken);
    }
}