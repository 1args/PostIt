using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PostIt.Infrastructure.Extensions;

internal static class HangfireExtensions
{
    public static IServiceCollection AddHangfireConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Not a good idea use redis
        var connectionString = configuration.GetConnectionString("RedisConnection");

        services.AddHangfire(config =>
        {
            config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseRedisStorage(connectionString, new RedisStorageOptions
                {
                    Db = 1,
                    Prefix = "hangfire:",
                    InvisibilityTimeout = TimeSpan.FromHours(1)
                });
        });
        
        services.AddHangfireServer();
        
        return services;
    }
}