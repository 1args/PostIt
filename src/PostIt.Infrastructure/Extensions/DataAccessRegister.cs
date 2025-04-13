using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PostIt.Infrastructure.Configuration;

namespace PostIt.Infrastructure.Extensions;

public static class DataAccessRegister
{
    public static IServiceCollection AddDataAccess<TDbContext, TDbContextOptionsConfigurator>(
        this IServiceCollection services) 
        where TDbContext : DbContext
        where TDbContextOptionsConfigurator : class, IDbContextOptionsConfigurator<TDbContext>
    {
        services.AddEntityFrameworkNpgsql()
            .AddDbContextPool<TDbContext>(Configure<TDbContext>);

        services.AddSingleton<IDbContextOptionsConfigurator<TDbContext>, TDbContextOptionsConfigurator>();
        services.AddScoped<DbContext>(serviceProvider => serviceProvider.GetRequiredService<TDbContext>());
        
        return services;
    }

    private static void Configure<TDbContext>(
        IServiceProvider serviceProvider, 
        DbContextOptionsBuilder optionsBuilder)
        where TDbContext : DbContext
    {
        var configurator = serviceProvider
            .GetRequiredService<IDbContextOptionsConfigurator<TDbContext>>();
        
        configurator.Configure((DbContextOptionsBuilder<TDbContext>)optionsBuilder);
    }
}