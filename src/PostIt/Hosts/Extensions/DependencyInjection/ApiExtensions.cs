using PostIt.Hosts.Features.HealthCheck;

namespace PostIt.Hosts.Extensions.DependencyInjection;

/// <summary>
/// Extension for registering Api level services.
/// </summary>
public static class ApiExtensions
{
    /// <summary>
    /// Registers Api level services.
    /// </summary>
    /// <returns>Collection of services.</returns>
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOpenApi()
            .AddHttpContextAccessor();

        services
            .AddSerilog(configuration)
            .AddAuthenticationRules(configuration)
            .UseHealthCheck(configuration)
            .AddGlobalExceptionHandler();
        
        return services;
    }
}