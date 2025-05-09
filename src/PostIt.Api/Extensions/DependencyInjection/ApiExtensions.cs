using PostIt.Application.Abstractions.Auth;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Services;
using PostIt.Infrastructure.Auth;
using PostIt.Infrastructure.Communication.Email;

namespace PostIt.Api.Extensions.DependencyInjection;

public static class ApiExtensions
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOpenApi()
            .AddHttpContextAccessor();

        services
            .AddSerilog(configuration)
            .AddAuthenticationRules(configuration)
            .AddGlobalExceptionHandler();
        
        return services;
    }
}