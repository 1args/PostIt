using Microsoft.Extensions.DependencyInjection;

namespace PostIt.Infrastructure.Extensions;

public static class AuthenticationRegister
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services)
    {
        return services;
    }
}