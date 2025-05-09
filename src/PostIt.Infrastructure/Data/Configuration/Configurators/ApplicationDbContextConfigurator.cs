using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PostIt.Infrastructure.Data.Configuration.Base;
using PostIt.Infrastructure.Data.Context;

namespace PostIt.Infrastructure.Data.Configuration.Configurators;

public class ApplicationDbContextConfigurator(
    IConfiguration configuration,
    ILoggerFactory loggerFactory)
    : DbContextOptionsConfigurator<ApplicationDbContext>(configuration, loggerFactory)
{
    public override string ConnectionStringName => "DefaultConnection";
}