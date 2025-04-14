using Microsoft.EntityFrameworkCore;
using PostIt.Application.Contracts.Requests;
using PostIt.Application.Contracts.Responses;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects.User;
using PostIt.Infrastructure.Configuration.Repositories;

namespace PostIt.Application.Services;

public class UserService(
    IRepository<User> userRepository)
{
    public async Task CreateUserAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var name = Name.Create(request.Name);
        var bio = Bio.Create(request.Bio);
        var email = Email.Create(request.Email);
        var password = Password.Create(request.Password);
        
        var user = User.Create(name, bio, email, password, request.Role, DateTime.UtcNow);
        
        await userRepository.AddAsync(user, cancellationToken);
    }

    public async Task<UserResponse> GetUserByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.Where(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            throw new InvalidOperationException($"User with id {userId} not found");
        }
        
        return MapToResponse(user);
    }

    public async Task DeleteUserAsync(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            throw new InvalidOperationException($"User with id {userId} not found");
        }

        await userRepository.DeleteAsync([user], cancellationToken);
    }

    public async Task UpdateUserBioAsync(
        UpdateBioRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository
            .Where(u => u.Id == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (user is null)
        {
            throw new InvalidOperationException($"User with id {request.UserId} not found");
        }

        var newBio = Bio.Create(request.Bio);
        user.UpdateBio(newBio);
        
        await userRepository.UpdateAsync(user, cancellationToken);
    }

    private static UserResponse MapToResponse(User user) =>
        new(user.Id,
            user.Name.Value,
            user.Bio.Value,
            user.Email.Value,
            user.Role,
            user.Posts,
            user.Comments,
            user.CreatedAt);
}