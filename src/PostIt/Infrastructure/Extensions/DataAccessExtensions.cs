using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PostIt.Application.Abstractions.Data;
using PostIt.Infrastructure.Data.Context;
using PostIt.Infrastructure.Data.Context.Repositories;

namespace PostIt.Infrastructure.Extensions;

/// <summary>
/// Extension for setting up Entity Framework Core with custom DbContext configurators and repositories.
/// </summary>
internal static class DataAccessExtensions
{
    /// <summary>
    /// Configures the database context, repositories, and options configurator for the specified DbContext type.
    /// </summary>
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        
        services.AddDbContextPool<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"), 
                    o => o.CommandTimeout(60))
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(loggerFactory);
        });
        
        services.AddScoped<DbContext, ApplicationDbContext>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        return services;
    }
}