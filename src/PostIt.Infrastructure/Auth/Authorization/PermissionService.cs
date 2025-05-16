using Microsoft.EntityFrameworkCore;
using PostIt.Application.Abstractions.Auth.Authorization;
using PostIt.Application.Abstractions.Data;
using PostIt.Domain.Entities;
using Permission = PostIt.Domain.Enums.Permission;

namespace PostIt.Infrastructure.Auth.Authorization;

/// <inheritdoc/>
public class PermissionService(
    IRepository<User> userRepository) : IPermissionService
{
    /// <inheritdoc/>
    public async Task<HashSet<Permission>> GetUserPermissionsAsync(Guid userId)
    {
        var roles = await userRepository
            .AsQueryable()
            .AsNoTracking()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.Id == userId)
            .Select(u => u.Roles)
            .ToArrayAsync();

        var permissions = roles
            .SelectMany(r => r)
            .SelectMany(r => r.Permissions)
            .Select(p => (Permission)p.Id)
            .ToHashSet();
        
        return permissions;
    }
}