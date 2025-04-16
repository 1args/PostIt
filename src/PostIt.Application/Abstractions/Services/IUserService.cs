using PostIt.Application.Contracts.Requests.User;
using PostIt.Application.Contracts.Responses;

namespace PostIt.Application.Abstractions.Services;

public interface IUserService
{
    Task<Guid> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken);

    Task<UserResponse> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    
    Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken);
    
    Task UpdateUserBioAsync(UpdateUserBioRequest request, CancellationToken cancellationToken);
}