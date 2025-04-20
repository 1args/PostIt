using Microsoft.EntityFrameworkCore;

namespace PostIt.Infrastructure.Data.Configuration.Base;

public interface IDbContextOptionsConfigurator<TDbContext> 
    where TDbContext : DbContext
{
    void Configure(DbContextOptionsBuilder<TDbContext> optionsBuilder);
}