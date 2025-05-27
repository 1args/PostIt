using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PostIt.Infrastructure.Extensions;

internal static class CacheExtensions
{
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