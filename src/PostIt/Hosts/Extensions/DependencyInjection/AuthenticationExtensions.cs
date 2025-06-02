using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PostIt.Contracts.Options;

namespace PostIt.Hosts.Extensions.DependencyInjection;

/// <summary>
/// Extension to configure JWT-based authentication.
/// </summary>
internal static class AuthenticationExtensions
{
    /// <summary>
    /// Adds JWT authentication.
    /// </summary>
    public static IServiceCollection AddAuthenticationRules(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()
            ?? throw new InvalidOperationException("JWTOptions is missing.");

        var validationOptions = jwtOptions.TokenValidationOptions;
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = validationOptions.ValidateIssuer,
                    ValidateAudience = validationOptions.ValidateAudience,
                    ValidateIssuerSigningKey = validationOptions.ValidateIssuerSigningKey,
                    ClockSkew = validationOptions.ClockSkew,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
                };
            });
        
        services.AddAuthorization();
        
        return services;
    }
}