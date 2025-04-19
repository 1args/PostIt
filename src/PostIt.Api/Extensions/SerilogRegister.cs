using Serilog;

namespace PostIt.Api.Extensions;

public static class SerilogRegister
{
    public static IServiceCollection AddSerilog(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddLogging(builder => builder.AddSerilog(new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger()));
        
        return services;
    }
}