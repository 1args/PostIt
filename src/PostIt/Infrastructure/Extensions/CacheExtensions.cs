using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PostIt.Infrastructure.Extensions;

/// <summary>
/// Extension methods for configuring caching.
/// </summary>
internal static class CacheExtensions
{
    /// <summary>
    /// Adds Redis distributed cache.
    /// </summary>
    public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "PostIt_";
        });

        services.AddDistributedMemoryCache();
        
        return services;
    }
}