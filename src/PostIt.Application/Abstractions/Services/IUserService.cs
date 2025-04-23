using Microsoft.AspNetCore.Http;
using PostIt.Contracts.ApiContracts.Requests.User;
using PostIt.Contracts.ApiContracts.Responses;

namespace PostIt.Application.Abstractions.Services;

public interface IUserService
{
    Task<Guid> RegisterAsync(CreateUserRequest request, CancellationToken cancellationToken);

    Task<(string acessToken, string refreshToken)> LoginAsync(string email, string password,
        CancellationToken cancellationToken);

    Task LogoutAsync(HttpRequest request, HttpResponse response, CancellationToken cancellationToken);

    Task RefreshToken(HttpRequest request, HttpResponse response, CancellationToken cancellationToken);

    Task<UserResponse> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    
    Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken);
    
    Task UpdateUserBioAsync(Guid userId, UpdateUserBioRequest request, CancellationToken cancellationToken);
}