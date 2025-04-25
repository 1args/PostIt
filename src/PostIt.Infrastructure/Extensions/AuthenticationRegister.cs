using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostIt.Infrastructure.Options;
using PostIt.Infrastructure.Options.Jwt;

namespace PostIt.Infrastructure.Extensions;

public static class AuthenticationRegister
{
    public static IServiceCollection AddAuthenticationData(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        
        return services;
    }
}