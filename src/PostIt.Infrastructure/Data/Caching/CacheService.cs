using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using PostIt.Application.Abstractions.Data;

namespace PostIt.Infrastructure.Data.Caching;

public class CacheService(IDistributedCache redisCache) : ICacheService
{
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = [];
        
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken) 
        where T : class
    {
        var cachedValue = await redisCache .GetStringAsync(key, cancellationToken);

        return cachedValue is null 
            ? null 
            : JsonSerializer.Deserialize<T>(cachedValue);
    }
    
    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken) 
        where T : class
    {
        var cachedValue = JsonSerializer.Serialize(value);
        
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), 
            SlidingExpiration = TimeSpan.FromMinutes(3)               
        };
        
        await redisCache.SetStringAsync(key, cachedValue, cacheOptions, cancellationToken);
        
        CacheKeys.TryAdd(key, false);
    }
    
    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        await redisCache.RemoveAsync(key, cancellationToken);
        CacheKeys.TryRemove(key, out _);
    }

    public Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken)
    {
        var tasks = CacheKeys.Keys
            .Where(k => k.StartsWith(prefixKey))
            .Select(k => RemoveAsync(k, cancellationToken));
        
        return Task.WhenAll(tasks);
    }
}