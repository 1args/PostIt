using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostIt.Infrastructure.Data.Configuration.Configurators;
using PostIt.Infrastructure.Data.Context;

namespace PostIt.Infrastructure.Extensions;

public static class InfrastructureRegister
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataAccess<ApplicationDbContext, ApplicationDbContextConfigurator>();
        services.AddAuthenticationData(configuration);
        
        return services;
    }
}