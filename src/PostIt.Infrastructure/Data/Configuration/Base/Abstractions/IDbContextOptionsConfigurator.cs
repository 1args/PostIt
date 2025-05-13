using Microsoft.EntityFrameworkCore;

namespace PostIt.Infrastructure.Data.Configuration.Base.Abstractions;

/// <summary>
/// Database context configurator.
/// </summary>
/// <typeparam name="TDbContext">Database configurator.</typeparam>
public interface IDbContextOptionsConfigurator<TDbContext> 
    where TDbContext : DbContext
{
    /// <summary>
    /// Configuring the database context.
    /// </summary>
    /// <param name="optionsBuilder">Options builder.</param>
    void Configure(DbContextOptionsBuilder<TDbContext> optionsBuilder);
}