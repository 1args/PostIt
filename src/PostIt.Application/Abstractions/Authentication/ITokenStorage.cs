namespace PostIt.Application.Abstractions.Authentication;

/// <summary>
/// Determines the operation of the token in the storage.
/// </summary>
public interface ITokenStorage
{
    /// <summary>
    /// Retrieves the token and associated user identifier.
    /// </summary>
    /// <param name="token">Token string to look up.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Tuple containing the token and associated user ID.</returns>
    Task<(string token, Guid userId)> GetTokenAsync(string token, CancellationToken cancellationToken);
    
    /// <summary>
    /// Stores the specified token and its associated user identifier.
    /// </summary>
    /// <param name="token">Token string to store.</param>
    /// <param name="userId">ID of the user associated with the token.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SetTokenAsync(string token, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes the specified token from storage.
    /// </summary>
    /// <param name="token">Token string to remove.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task RemoveTokenAsync(string token, CancellationToken cancellationToken);
}