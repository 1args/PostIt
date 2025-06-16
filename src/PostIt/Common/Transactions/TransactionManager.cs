using Microsoft.EntityFrameworkCore;
using PostIt.Common.Abstractions;

namespace PostIt.Common.Transactions;

/// <inheritdoc/>
public class TransactionManager(
    DbContext dbContext) : ITransactionManager
{
    /// <summary>
    /// Database context.
    /// </summary>
    private DbContext DbContext { get; } = dbContext;

    /// <inheritdoc/>
    public async Task StartEffect(
        Func<Task> operation, 
        CancellationToken cancellationToken)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await operation();
            await transaction.CommitAsync(cancellationToken);
        }
        catch 
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<TResult> StartEffect<TResult>(
        Func<Task<TResult>> operation,
        CancellationToken cancellationToken)
    {
        await using var transaction = await DbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await operation();
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch 
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}