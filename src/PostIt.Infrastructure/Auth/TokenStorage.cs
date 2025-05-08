using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using PostIt.Application.Abstractions.Auth;
using PostIt.Application.Exceptions;
using PostIt.Infrastructure.Options;

namespace PostIt.Infrastructure.Auth; 

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
            throw new TokenExpiredException("The token has expired.");
        }
        if (!Guid.TryParse(storedGuid, out var userId))
        {
            throw new InvalidTokenException("Invalid token format.");
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