using Serilog;
using Serilog.Filters;

namespace PostIt.Api.Extensions.DependencyInjection;

internal static class SerilogExtensions
{
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