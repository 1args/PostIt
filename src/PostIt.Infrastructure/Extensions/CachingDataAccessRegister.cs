using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostIt.Application.Abstractions.Data;
using PostIt.Infrastructure.Data.Caching;

namespace PostIt.Infrastructure.Extensions;

public static class CachingDataAccessRegister
{
    public static IServiceCollection AddCachingDataAccess(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "PostIt_";
        });

        services.AddDistributedMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();
        
        return services;
    }
}