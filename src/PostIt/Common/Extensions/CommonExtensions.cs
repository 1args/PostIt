using Microsoft.Extensions.DependencyInjection;
using PostIt.Common.Abstractions;
using PostIt.Common.Transactions;

namespace PostIt.Common.Extensions;

/// <summary>
/// Extension for registering services common to all layers.
/// </summary>
public static class CommonExtensions
{
    /// <summary>
    /// Adds common services.
    /// </summary>
    public static IServiceCollection AddCommon(this IServiceCollection services)
    {
        services
            .AddScoped<ITransactionManager, TransactionManager>();
        
        return services;
    }
}