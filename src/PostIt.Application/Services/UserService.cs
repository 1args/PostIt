using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostIt.Application.Abstractions.Data;
using PostIt.Application.Abstractions.Services;
using PostIt.Contracts.ApiContracts.Requests.User;
using PostIt.Contracts.ApiContracts.Responses;
using PostIt.Contracts.Exceptions;
using PostIt.Contracts.Mappers;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects;

namespace PostIt.Application.Services;

/// <inheritdoc/>
public class UserService(
    IRepository<User> userRepository,
    IRepository<Role> roleRepository,
    IAuthenticationService authenticationService,
    ILogger<UserService> logger) : IUserService
{
    public async Task<UserResponse> GetCurrentUserAsync(
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        logger.LogInformation("Fetching user by ID `{UserId}`.", userId);

       var user = await GetUserOrThrowAsync(userId, tracking: false, cancellationToken: cancellationToken);

       return user.MapToPublic();
    }
    
    /// <inheritdoc/>
    public async Task<UserResponse> GetUserByIdAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching user by ID `{UserId}`.", userId);

        var user = await GetUserOrThrowAsync(userId, tracking: false, cancellationToken: cancellationToken);
        
        logger.LogInformation("User with ID `{UserId}` retrieved successfully.", userId);
        
        return user.MapToPublic();
    }

    /// <inheritdoc/>
    public async Task DeleteUserAsync(
        Guid userId, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting user with ID `{UserId}`.", userId);
        
        var user = await GetUserOrThrowAsync(userId, cancellationToken);

        await userRepository.DeleteAsync([user], cancellationToken);
        logger.LogInformation("User with ID `{UserId}` deleted successfully.", userId);
    }

    /// <inheritdoc/>
    public async Task UpdateUserBioAsync(
        Guid userId,
        UpdateUserBioRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating bio for user with ID `{UserId}`.", userId);
        
        var user = await GetUserOrThrowAsync(userId, cancellationToken);

        var newBio = UserBio.Create(request.Bio);
        user.UpdateBio(newBio);
        
        await userRepository.UpdateAsync(user, cancellationToken);
        
        logger.LogInformation("Bio updated successfully for user with ID `{UserId}`.",
            userId);
    }

    /// <inheritdoc/>
    public async Task RestrictUserAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to restrict user with ID `{UserId}`...", userId);
        
        var currentUserId = GetCurrentUserId();
        
        if (userId == currentUserId)
        {
            logger.LogWarning("Moderator with ID `{UserId}` attempted to restrict themselves.", userId);
            throw new ForbiddenException("Cannot restrict yourself.");
        }
        
        var user = await GetUserOrThrowAsync(userId, cancellationToken);
        
        if (user.Roles.Any(r => r.Id is (int)Domain.Enums.Role.Moderator or (int)Domain.Enums.Role.Admin))
        {
            logger.LogWarning(
                "Moderator with ID `{ModeratorId}` attempted to restrict a moderator with ID `{UserId}`.", 
                currentUserId,
                userId);
            throw new ForbiddenException("Cannot restrict moderators.");
        }
        
        var restrictedRole = await roleRepository
            .AsQueryable()
            .SingleOrDefaultAsync(r => r.Id == (int)Domain.Enums.Role.Restricted, cancellationToken)
            ?? throw new InvalidOperationException($"Role {Domain.Enums.Role.Restricted} does not exist.");

        if (user.Roles.Any(r => r.Id == restrictedRole.Id))
        {
            logger.LogWarning("User with ID `{UserId}` is already restricted.", userId);
            throw new ForbiddenException("User is already restricted.");
        }
        
        user.AddRole(restrictedRole);
        
        await userRepository.UpdateAsync(user, cancellationToken);
        
        logger.LogInformation(
            "User with ID `{UserId}` restricted successfully by moderator `{Moderator}`.", 
            userId,
            currentUserId);
    }
    
    /// <inheritdoc/>
    public async Task UnrestrictUserAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to unrestrict user with ID `{UserId}`...", userId);

        var currentUserId = GetCurrentUserId();
        
        if (userId == currentUserId)
        {
            logger.LogWarning("Moderator with ID `{UserId}` attempted to unrestrict themselves.", userId);
            throw new ForbiddenException("Cannot unrestrict yourself.");
        }
        
        var user = await GetUserOrThrowAsync(userId, cancellationToken);
        
        var restrictedRole = await roleRepository
            .AsQueryable()
            .SingleOrDefaultAsync(r => r.Id == (int)Domain.Enums.Role.Restricted, cancellationToken)
            ?? throw new InvalidOperationException($"Role {Domain.Enums.Role.Restricted} does not exist.");
        
        var hasRestrictedRole = user.Roles.SingleOrDefault(r => r.Id == restrictedRole.Id);
        
        if (hasRestrictedRole is null)
        {
            logger.LogWarning("User with ID `{UserId}` is not restricted.", userId);
            throw new ConflictException("User is not restricted.");
        }

        user.RemoveRole(restrictedRole);
        
        await userRepository.UpdateAsync(user, cancellationToken);

        logger.LogInformation(
            "User with ID `{UserId}` unrestricted successfully by moderator `{Moderator}`.",
            userId,
            currentUserId);
    }

    /// <inheritdoc/>
    public async Task AssignModeratorRoleAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to assign moderator role to user with ID `{UserId}`...", userId);
        
        var currentUserId = GetCurrentUserId();
        
        if (userId == currentUserId)
        {
            logger.LogWarning("Admin with ID `{UserId}` attempted to assign moderator role to themselves.", userId);
            throw new ForbiddenException("Cannot assign moderator role to yourself.");
        }
        
        var user = await GetUserOrThrowAsync(userId, cancellationToken);
        
         var moderatorRole = await roleRepository
            .AsQueryable()
            .SingleOrDefaultAsync(r => r.Id == (int)Domain.Enums.Role.Moderator, cancellationToken)
            ?? throw new InvalidOperationException($"Role {Domain.Enums.Role.Moderator} does not exist.");
        
        if (user.Roles.Any(r => r.Id == moderatorRole.Id))
        {
            logger.LogWarning("User with ID `{UserId}` already has the moderator role.", userId);
            throw new ConflictException("User is already a moderator.");
        }
        
        user.AddRole(moderatorRole);
        
        await userRepository.UpdateAsync(user, cancellationToken);
        
        logger.LogInformation(
            "Moderator role assigned successfully to user with ID `{UserId}` by admin `{Admin}`.",
            userId,
            currentUserId);
    }

    /// <inheritdoc/>
    public async Task UnassignModeratorRoleAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Attempting to remove moderator role from user with ID `{UserId}`...", userId);

        var currentUserId = GetCurrentUserId();
        
        if (userId == currentUserId)
        {
            logger.LogWarning("Admin with ID `{UserId}` attempted to remove moderator role from themselves.", userId);
            throw new ForbiddenException("Cannot remove moderator role from yourself.");
        }
        
        var user = await GetUserOrThrowAsync(userId, cancellationToken);
        
        var moderatorRole = await roleRepository
            .AsQueryable()
            .SingleOrDefaultAsync(r => r.Id == (int)Domain.Enums.Role.Moderator, cancellationToken)
            ?? throw new InvalidOperationException($"Role {Domain.Enums.Role.Moderator} does not exist.");

        var userModeratorRole = user.Roles.FirstOrDefault(r => r.Id == moderatorRole.Id);
        
        if (userModeratorRole is null)
        {
            logger.LogWarning("User with ID `{UserId}` is not a moderator.", userId);
            throw new ConflictException("User is not a moderator.");
        }
        
        user.RemoveRole(moderatorRole);
        
        await userRepository.UpdateAsync(user, cancellationToken);
        
        logger.LogInformation(
            "Moderator role removed successfully from user with ID `{UserId}` by admin `{Admin}`.",
            userId,
            currentUserId);
    }
    
    private Guid GetCurrentUserId() => authenticationService.GetUserIdFromAccessToken();
    
    private async Task<User> GetUserOrThrowAsync(
        Guid userId,
        CancellationToken cancellationToken,
        bool tracking = true)
    {
        var query = userRepository.AsQueryable();
        
        query = tracking ? query.AsTracking() : query.AsNoTracking();
        
        var user = await query
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
        {
            logger.LogWarning("User with ID `{UserId}` not found.", userId);
            throw new NotFoundException($"User with ID '{userId}' not found.");
        }

        logger.LogInformation("User with ID `{UserId}` retrieved successfully.", userId);
        
        return user;
    }
}