using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostIt.Application.Abstractions.Auth;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Services;
using PostIt.Infrastructure.Auth;
using PostIt.Infrastructure.Communication.Email;
using PostIt.Infrastructure.Options;

namespace PostIt.Infrastructure.Extensions;

internal static class AuthenticationExtensions
{
    public static IServiceCollection AddAuthRegister(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        
        services
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<ITokenStorage, TokenStorage>()
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<IPasswordHasher, PasswordHasher>()
            .AddScoped<IJwtProvider, JwtProvider>()
            .AddScoped<IEmailVerificationLinkFactory, EmailVerificationLinkFactory>();
        
        return services;
    }
}