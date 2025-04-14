using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PostIt.Infrastructure.Configuration.Base;
using PostIt.Infrastructure.Context;

namespace PostIt.Infrastructure.Configuration.Configurators;

public class ApplicationDbContextConfigurator(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : DbContextOptionsConfigurator<ApplicationDbContext>(configuration, loggerFactory)
{
    public override string ConnectionStringName => "DefaultConnection";
}