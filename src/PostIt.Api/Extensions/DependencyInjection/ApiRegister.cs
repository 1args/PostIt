using PostIt.Application.Abstractions.Auth;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Services;
using PostIt.Infrastructure.Auth;
using PostIt.Infrastructure.Communication.Email;

namespace PostIt.Api.Extensions.DependencyInjection;

public static class ApiRegister
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITokenStorage, TokenStorage>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IEmailVerificationLinkFactory, EmailVerificationLinkFactory>();
        
        return services;
    }
}