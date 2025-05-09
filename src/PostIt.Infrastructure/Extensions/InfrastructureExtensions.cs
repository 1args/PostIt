using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostIt.Infrastructure.Data.Configuration.Configurators;
using PostIt.Infrastructure.Data.Context;

namespace PostIt.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDataAccess<ApplicationDbContext, ApplicationDbContextConfigurator>()
            .AddCaching(configuration)
            .AddAuthRegister(configuration)
            .AddHangfireConfiguration(configuration)
            .AddMinio(configuration)
            .AddSmtpConfiguration(configuration);
        
        return services;
    }
}