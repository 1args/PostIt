using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PostIt.Application.Abstractions.Data;
using PostIt.Infrastructure.Data.Configuration.Base;
using PostIt.Infrastructure.Data.Configuration.Repositories;

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
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
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