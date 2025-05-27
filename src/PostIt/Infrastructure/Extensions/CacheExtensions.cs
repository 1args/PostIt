using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostIt.Application.Abstractions.Data;
using PostIt.Application.Abstractions.Services;
using PostIt.Infrastructure.Data.Caching;

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
        services.AddSingleton<ICacheService, CacheService>();
        
        return services;
    }
}