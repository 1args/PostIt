using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Contracts.Requests.User;
using PostIt.Application.Contracts.Responses;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects.User;
using PostIt.Infrastructure.Configuration.Repositories;

namespace PostIt.Application.Services;

public class UserService(
    IRepository<User> userRepository,
    ILogger<UserService> logger) : IUserService
{
    public async Task<Guid> CreateUserAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating user with email: {Email}.", request.Email);
        
        var name = Name.Create(request.Name);
        var bio = Bio.Create(string.Empty);
        var email = Email.Create(request.Email);
        var password = Password.Create(request.Password);
        
        var user = User.Create(name, bio, email, password, request.Role, DateTime.UtcNow);
        
        await userRepository.AddAsync(user, cancellationToken);
        logger.LogInformation("User created successfully. User ID: {Id}.", user.Id);
        
        return user.Id;
    }

    public async Task<UserResponse> GetUserByIdAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting user by ID {Id}.", userId);

        var user = await GetUserOrThrowAsync(userId, cancellationToken);
        
        logger.LogInformation("Retrieved user by ID {Id} retrieved successfully.", userId);
        return MapToResponse(user);
    }

    public async Task DeleteUserAsync(
        Guid userId, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting user with ID: {UserId}.", userId);
        
        var user = await GetUserOrThrowAsync(userId, cancellationToken);

        await userRepository.DeleteAsync([user], cancellationToken);
        logger.LogInformation("User with ID {Id} deleted successfully.", userId);
    }

    public async Task UpdateUserBioAsync(
        UpdateUserBioRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating bio for user with ID: {Id}", request.UserId);
        
        var user = await GetUserOrThrowAsync(request.UserId, cancellationToken);

        var newBio = Bio.Create(request.Bio);
        user.UpdateBio(newBio);
        
        await userRepository.UpdateAsync(user, cancellationToken);
        logger.LogInformation("Bio updated successfully for user with ID: {Id}", request.UserId);
    }

    private async Task<User> GetUserOrThrowAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var user = await userRepository
            .Where(u => u.Id == userId)
            .SingleOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            logger.LogWarning("User with ID {Id} not found.", userId);
            throw new InvalidOperationException($"User with ID {userId} not found.");
        }

        return user;
    }
    private static UserResponse MapToResponse(User user) =>
        new(user.Id,
            user.Name.Value,
            user.Bio.Value,
            user.Role,
            user.Posts,
            user.Comments,
            user.CreatedAt);
}