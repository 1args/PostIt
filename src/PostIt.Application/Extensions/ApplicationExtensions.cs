using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Services;
using PostIt.Application.Validators.User;

namespace PostIt.Application.Extensions;

/// <summary>
/// Extensions for adding application level classes.
/// </summary>
public static class ApplicationExtensions
{
    /// <summary>
    /// Registers Application level classes.
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
        
        return services;
    }
}