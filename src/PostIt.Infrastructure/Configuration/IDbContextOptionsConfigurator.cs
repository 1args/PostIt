using Microsoft.EntityFrameworkCore;

namespace PostIt.Infrastructure.Configuration;

public interface IDbContextOptionsConfigurator<TDbContext> 
    where TDbContext : DbContext
{
    void Configure(DbContextOptionsBuilder<TDbContext> optionsBuilder);
}