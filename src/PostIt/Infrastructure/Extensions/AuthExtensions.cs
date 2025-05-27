using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostIt.Application.Abstractions.Auth.Authentication;
using PostIt.Application.Abstractions.Auth.Authorization;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Services;
using PostIt.Contracts.Options;
using PostIt.Infrastructure.Auth.Authentication;
using PostIt.Infrastructure.Auth.Authorization;
using PostIt.Infrastructure.Messaging;
using AuthorizationOptions = PostIt.Contracts.Options.AuthorizationOptions;

namespace PostIt.Infrastructure.Extensions;

internal static class AuthExtensions
{
    public static IServiceCollection AddAuthRegister(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)))
            .Configure<AuthorizationOptions>(configuration.GetSection(nameof(AuthorizationOptions)));
        
        services
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<ITokenStorage, TokenStorage>()
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<IPasswordHasher, PasswordHasher>()
            .AddScoped<IJwtProvider, JwtProvider>()
            .AddScoped<IEmailVerificationLinkFactory, EmailVerificationLinkFactory>();

        services.AddScoped<IPermissionService, PermissionService>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        
        return services;
    }
}