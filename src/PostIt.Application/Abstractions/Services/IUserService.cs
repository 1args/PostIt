using Microsoft.AspNetCore.Http;
using PostIt.Contracts.ApiContracts.Requests.User;
using PostIt.Contracts.ApiContracts.Responses;

namespace PostIt.Application.Abstractions.Services;

public interface IUserService
{
    Task<Guid> RegisterAsync(CreateUserRequest request, CancellationToken cancellationToken);

    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

    Task<bool> VerifyEmailAsync(Guid userId, Guid token, CancellationToken cancellationToken);

    Task LogoutAsync(CancellationToken cancellationToken);

    Task<LoginResponse> RefreshToken(CancellationToken cancellationToken);

    Task UploadAvatarAsync(ReadOnlyMemory<byte> avatar, CancellationToken cancellationToken);

    Task<UserResponse> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    
    Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken);
    
    Task UpdateUserBioAsync(Guid userId, UpdateUserBioRequest request, CancellationToken cancellationToken);
}