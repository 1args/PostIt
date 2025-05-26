using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PostIt.Infrastructure.Data.Configuration.Base;
using PostIt.Infrastructure.Data.Context;

namespace PostIt.Infrastructure.Data.Configuration.Configurators;

/// <summary>
/// Configurator of the main database context.
/// </summary>
public sealed class ApplicationDbContextConfigurator(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : DbContextOptionsConfigurator<ApplicationDbContext>(configuration, loggerFactory)
{
    /// <inheritdoc/>
    protected override string ConnectionStringName => "DefaultConnection";
}