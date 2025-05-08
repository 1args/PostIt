using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PostIt.Application.Abstractions.Auth;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Services;
using PostIt.Application.Validators.User;

namespace PostIt.Application.Extensions;

public static class ApplicationExtensions
{
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