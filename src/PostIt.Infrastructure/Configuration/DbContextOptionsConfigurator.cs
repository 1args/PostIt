using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PostIt.Infrastructure.Configuration;

public abstract class DbContextOptionsConfigurator<TDbContext>(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : IDbContextOptionsConfigurator<TDbContext>
    where TDbContext : DbContext
{
    public abstract string ConnectionStringName { get; }
    
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
            .UseNpgsql(connectionString, x => x.CommandTimeout(60));
    }
}