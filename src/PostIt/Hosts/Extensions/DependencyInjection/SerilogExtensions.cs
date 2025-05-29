using Serilog;
using Serilog.Filters;

namespace PostIt.Hosts.Extensions.DependencyInjection;

/// <summary>
/// Extension for configuring Serilog logging.
/// </summary>
internal static class SerilogExtensions
{
    /// <summary>
    /// Configures Serilog with console and OpenSearch sinks.
    /// </summary>
    public static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(builder => builder.AddSerilog(new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Logger(lc => lc
                .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                .WriteTo.OpenSearch(
                    configuration.GetConnectionString("OpenSearchConnection"),
                    "postit-logs-{0:yyyy.MM.dd}"))
            .WriteTo.Logger(lc => lc.WriteTo.Console())
            .CreateLogger()));
        
        return services;
    }
}