using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostIt.Infrastructure.Data.Configuration.Configurators;
using PostIt.Infrastructure.Data.Context;

namespace PostIt.Infrastructure.Extensions;

/// <summary>
/// Extension for registering Infrastructure level services.
/// </summary>
public static class InfrastructureExtensions
{
    /// <summary>
    /// Registers Infrastructure level services.
    /// </summary>
    /// <returns>Collection of services.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDataAccess<ApplicationDbContext, ApplicationDbContextConfigurator>()
            .AddCaching(configuration)
            .AddAuthRegister(configuration)
            .AddMinio(configuration)
            .AddSmtpConfiguration(configuration);
        
        return services;
    }
}