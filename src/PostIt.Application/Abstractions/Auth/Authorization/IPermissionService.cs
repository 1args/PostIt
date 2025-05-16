using PostIt.Domain.Enums;

namespace PostIt.Application.Abstractions.Auth.Authorization;

/// <summary>
/// Provides functionality for retrieving user permissions.
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Retrieves the set of permissions granted to a specific user.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <returns>Set of <see cref="Permission"/> values.</returns>
    Task<HashSet<Permission>> GetUserPermissionsAsync(Guid userId);
}