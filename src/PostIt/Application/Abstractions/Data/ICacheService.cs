namespace PostIt.Application.Abstractions.Data;

/// <summary>
/// Provides functionality for caching data.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Retrieves a value from cache using the specified key.
    /// </summary>
    /// <param name="key">Cache key.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <typeparam name="T">Type of the cached object.</typeparam>
    /// <returns>Cached object or null if not found.</returns>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken) where T : class;
    
    /// <summary>
    /// Stores a value in the cache under the specified key.
    /// </summary>
    /// <param name="key">Cache key.</param>
    /// <param name="value">Object to cache.</param>
    /// /// <param name="expiry">Object lifetime.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <typeparam name="T">Type of the object being cached.</typeparam>
    Task SetAsync<T>(string key, T value, TimeSpan? expiry, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Removes a cached value by key.
    /// </summary>
    /// <param name="key">Cache key.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task RemoveAsync(string key, CancellationToken cancellationToken);
    
    /// <summary>
    /// Removes all cached entries that start with the specified pattern.
    /// </summary>
    /// <param name="pattern">Pattern used to match multiple cache keys.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken);
}