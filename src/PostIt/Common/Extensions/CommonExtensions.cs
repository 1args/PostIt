using Microsoft.Extensions.DependencyInjection;
using PostIt.Common.Abstractions;
using PostIt.Common.Transactions;

namespace PostIt.Common.Extensions;

public static class CommonExtensions
{
    public static IServiceCollection AddCommon(this IServiceCollection services)
    {
        services
            .AddScoped<ITransactionManager, TransactionManager>();
        
        return services;
    }
}