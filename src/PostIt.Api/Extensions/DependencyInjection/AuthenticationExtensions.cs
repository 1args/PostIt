using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PostIt.Infrastructure.Options;

namespace PostIt.Api.Extensions.DependencyInjection;

internal static class AuthenticationExtensions
{
    public static IServiceCollection AddAuthenticationRules(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()
            ?? throw new InvalidOperationException("JWTOptions is missing.");

        var validationOptions = jwtOptions.TokenValidationOptions;

        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
        
        return services;
    }
    
}