using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Services;
using PostIt.Application.Validators.User;

namespace PostIt.Application.Extensions;

public static class ApplicationRegister
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICommentService, CommentService>();

        services
            .AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
        
        return services;
    }
}