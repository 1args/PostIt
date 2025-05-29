using Microsoft.AspNetCore.Http.Features;
using PostIt.Hosts.ErrorHandling;

namespace PostIt.Hosts.Extensions.DependencyInjection;

/// <summary>
/// Extension to configure global exception handling.
/// </summary>
internal static class  ExceptionHandlerExtensions
{
    /// <summary>
    /// Registers a global exception handler and configures ProblemDetails for consistent error responses.
    /// </summary>
    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                
                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
            };
        });
        
        services.AddExceptionHandler<GlobalExceptionHandler>();
        
        return services;
    }
}