using Microsoft.Extensions.Caching.Hybrid;
using PostIt.Application.Abstractions.Data;
using StackExchange.Redis;

namespace PostIt.Infrastructure.Data.Caching;

/// <inheritdoc/>
public class CacheService(
    HybridCache hybridCache,
    IConnectionMultiplexer redis) : ICacheService
{
    /// <inheritdoc/>
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken) where T : class
    {
        return await hybridCache.GetOrCreateAsync(
            key,
            ct => ValueTask.FromResult(default(T)),
            new HybridCacheEntryOptions { Expiration = TimeSpan.FromMinutes(5) },
            cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry, CancellationToken cancellationToken) where T : class
    {
        var options = new HybridCacheEntryOptions { Expiration = expiry ?? TimeSpan.FromMinutes(5) };
        
        await hybridCache.GetOrCreateAsync(
            key,
            ct => ValueTask.FromResult(value),
            options,
            cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        await hybridCache.RemoveAsync(key, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken)
    {
        var server = redis.GetServer(redis.GetEndPoints()
            .First());
        
        var keys = server.Keys(pattern: pattern)
            .ToArray();

        foreach (var key in keys)
        {
            await hybridCache.RemoveAsync(key!, cancellationToken);
        }
    }
}