using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Abstractions.Utilities;
using PostIt.Application.Services;
using PostIt.Application.Utilities;
using PostIt.Application.Validators.User;

namespace PostIt.Application.Extensions;

/// <summary>
/// Extension for registering Application level services.
/// </summary>
public static class ApplicationExtensions
{
    /// <summary>
    /// Registers Application level services.
    /// </summary>
    /// <returns>Collection of services.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddScoped<IUserService, UserService>()
            .AddScoped<IPostService, PostService>()
            .AddScoped<ICommentService, CommentService>()
            .AddScoped<IEmailVerificationService, EmailVerificationService>()
            .AddScoped<IAvatarService, AvatarService>();
        
        services
            .AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
        
        services.AddScoped(typeof(IPermissionChecker<>), typeof(PermissionChecker<>));
        
        return services;
    }
}