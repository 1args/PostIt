using Serilog;

namespace PostIt.Api.Extensions.DependencyInjection;

internal static class SerilogExtensions
{
    public static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(builder => builder.AddSerilog(new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger()));
        
        return services;
    }
}