namespace PostIt.Common.Abstractions;

/// <summary>
/// Provides database transaction management.
/// </summary>
public interface ITransactionManager
{
    /// <summary>
    /// Executes operation within a database transaction.
    /// </summary>
    /// <param name="operation">Operation to execute.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns></returns>
    Task StartEffect(Func<Task> operation, CancellationToken cancellationToken);

    /// <summary>
    /// Executes operation within a database transaction.
    /// </summary>
    /// <typeparam name="TResult">Type of the result returned by the operation.</typeparam>
    /// <param name="operation">Operation to execute.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns></returns>
    Task<TResult> StartEffect<TResult>(Func<Task<TResult>> operation, CancellationToken cancellationToken);
}