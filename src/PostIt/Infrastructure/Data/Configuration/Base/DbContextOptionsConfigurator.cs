using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PostIt.Infrastructure.Data.Configuration.Base.Abstractions;

namespace PostIt.Infrastructure.Data.Configuration.Base;

/// <inheritdoc/>
public abstract class DbContextOptionsConfigurator<TDbContext>(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : IDbContextOptionsConfigurator<TDbContext>
    where TDbContext : DbContext
{
    /// <summary>
    /// Connection string.
    /// </summary>
    protected abstract string ConnectionStringName { get; }
    
    /// <inheritdoc/>
    public void Configure(DbContextOptionsBuilder<TDbContext> optionsBuilder)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"Connection string '{ConnectionStringName}' does not exist.");
        }

        optionsBuilder
            .UseLoggerFactory(loggerFactory)
            .UseNpgsql(connectionString, o =>
            {
                o.CommandTimeout(60);
                o.MigrationsAssembly("Migrations");
            });
    }
}