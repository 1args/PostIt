namespace PostIt.Application.Abstractions.Auth;

public interface ITokenStorage
{
    Task<(string token, Guid userId)> GetTokenAsync(string token, CancellationToken cancellationToken);
    
    Task SetTokenAsync(string token, Guid userId, CancellationToken cancellationToken);

    Task RemoveTokenAsync(string token, CancellationToken cancellationToken);
}