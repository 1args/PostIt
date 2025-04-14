using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostIt.Application.Contracts.Requests;
using PostIt.Application.Contracts.Responses;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects.User;
using PostIt.Infrastructure.Configuration.Repositories;

namespace PostIt.Application.Services;

public class UserService(
    IRepository<User> userRepository,
    ILogger<UserService> logger)
{
    public async Task CreateUserAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating user with email: {Email}.", request.Email);
        
        var name = Name.Create(request.Name);
        var bio = Bio.Create(request.Bio);
        var email = Email.Create(request.Email);
        var password = Password.Create(request.Password);
        
        var user = User.Create(name, bio, email, password, request.Role, DateTime.UtcNow);
        
        await userRepository.AddAsync(user, cancellationToken);
        
        logger.LogInformation("User created successfully. User ID: {Id}.", user.Id);
    }

    public async Task<UserResponse> GetUserByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.Where(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken);
        
        logger.LogInformation("Getting user by ID {Id}.", userId);

        if (user is null)
        {
            logger.LogWarning("User with ID {Id} not found.", userId);
            throw new InvalidOperationException($"User with id {userId} not found.");
        }
        
        logger.LogInformation("Retrieved user by ID {Id} retrieved successfully.", userId);
        return MapToResponse(user);
    }

    public async Task DeleteUserAsync(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Deleting user with ID: {UserId}.", userId);
        
        var user = await userRepository
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            logger.LogWarning("User with ID {Id} not found for deletion.", userId);
            throw new InvalidOperationException($"User with id {userId} not found.");
        }

        await userRepository.DeleteAsync([user], cancellationToken);
        logger.LogInformation("User with ID {Id} deleted successfully.", userId);
    }

    public async Task UpdateUserBioAsync(
        UpdateBioRequest request,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Updating bio for user with ID: {Id}", request.UserId);
        
        var user = await userRepository
            .Where(u => u.Id == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (user is null)
        {
            logger.LogWarning("User with ID {Id} not found for bio update", request.UserId);
            throw new InvalidOperationException($"User with id {request.UserId} not found");
        }

        var newBio = Bio.Create(request.Bio);
        user.UpdateBio(newBio);
        
        await userRepository.UpdateAsync(user, cancellationToken);
        logger.LogInformation("Bio updated successfully for user with ID: {Id}", request.UserId);
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