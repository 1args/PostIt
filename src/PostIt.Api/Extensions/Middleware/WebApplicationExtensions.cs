using Hangfire;
using PostIt.Api.Extensions.Endpoints;
using Scalar.AspNetCore;

namespace PostIt.Api.Extensions.Middleware;

public static class WebApplicationExtensions
{
    public static WebApplication UseApiMiddlewares(this WebApplication app)
    {
        app.MapApiEndpoints();
        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }
        
        app
            .UseHttpsRedirection()
            .UseExceptionHandler()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseHangfireDashboard();
        
        return app;
;    }
}