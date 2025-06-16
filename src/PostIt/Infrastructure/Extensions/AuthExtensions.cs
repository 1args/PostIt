using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostIt.Application.Abstractions.Auth.Authorization;
using PostIt.Application.Abstractions.Security.Authentication;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Services;
using PostIt.Contracts.Options;
using PostIt.Infrastructure.Messaging;
using PostIt.Infrastructure.Security.Authentication;
using PostIt.Infrastructure.Security.Authorization;
using AuthorizationOptions = PostIt.Contracts.Options.AuthorizationOptions;

namespace PostIt.Infrastructure.Extensions;

/// <summary>
/// Extension to register authentication and authorization services.
/// </summary>
internal static class AuthExtensions
{
    /// <summary>
    /// Registers core authentication/authorization services, configuration, and handlers.
    /// </summary>
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