using Microsoft.Extensions.DependencyInjection;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Abstractions.Services.Services;
using PostIt.Application.Services;

namespace PostIt.Application.Extensions;

public static class ApplicationRegister
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICommentService, CommentService>();
        
        return services;
    }
}