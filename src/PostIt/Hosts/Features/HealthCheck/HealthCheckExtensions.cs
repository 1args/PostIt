using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PostIt.Api.Extensions.Endpoints;
using PostIt.Domain.Enums;

namespace PostIt.Hosts.Features.HealthCheck;

/// <summary>
/// Extension for working with health checks.
/// </summary>
public static class HealthCheckExtensions
{
    /// <summary>
    /// Registers health checks services.
    /// </summary>
    public static IServiceCollection UseHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        var postgresConnectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentException("Connection string not found for postgres.");
        
        var redisConnectionString = configuration.GetConnectionString("RedisConnection")
            ?? throw new ArgumentException("Connection string not found for redis.");

        services.AddHealthChecksUI(options =>
        {
            options.AddHealthCheckEndpoint("PostIt: Liveness Check", "/health-checks/live"); 
            options.AddHealthCheckEndpoint("PostIt: Readiness Check", "/health-checks/ready"); 
            options.SetEvaluationTimeInSeconds(10);
            options.MaximumHistoryEntriesPerEndpoint(50);
        }).AddInMemoryStorage();
        
        services
            .AddHealthChecks()
            .AddNpgSql(postgresConnectionString, name: "postgres", tags: ["readiness"], timeout: TimeSpan.FromSeconds(10))
            .AddRedis(redisConnectionString, name: "redis", tags: ["readiness"], timeout: TimeSpan.FromSeconds(5))
            .AddCheck("self", () => HealthCheckResult.Healthy("Api is running."), tags: ["liveness"]);
        
        return services;
    }

    /// <summary>
    /// Maps endpoints to check system health.
    /// </summary>
    public static WebApplication MapHealthChecksEndpoints(this WebApplication app)
    {
        app
            .MapHealthChecksUI()
            .RequireAuthorization()
            .RequirePermissions(Permission.SpecialAccess);
        
        app.MapHealthChecks("/health-checks/live", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("liveness"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).RequireAuthorization().RequirePermissions(Permission.SpecialAccess);
        
        app.MapHealthChecks("/health-checks/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("readiness"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).RequireAuthorization().RequirePermissions(Permission.SpecialAccess);
        
        return app;
    }
}