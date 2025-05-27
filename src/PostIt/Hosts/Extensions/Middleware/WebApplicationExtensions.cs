using PostIt.Api.Extensions.Endpoints;
using Scalar.AspNetCore;

namespace PostIt.Hosts.Extensions.Middleware;

/// <summary>
/// An extension for configuring middleware in WebApplication.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Configures middleware APIs.
    /// </summary>
    /// <returns>WebApplication instance.</returns>
    public static WebApplication UseApiMiddlewares(this WebApplication app)
    {
        app.MapApiEndpoints();
        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app
            .UseRouting()
            .UseHttpsRedirection()
            .UseExceptionHandler()
            .UseAuthentication()
            .UseAuthorization();
        
        return app;
;    }
}