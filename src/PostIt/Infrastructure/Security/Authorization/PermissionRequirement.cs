using Microsoft.AspNetCore.Authorization;
using PostIt.Domain.Enums;

namespace PostIt.Infrastructure.Security.Authorization;

/// <summary>
/// Represents a permission requirement used in authorization policies.
/// </summary>
/// <param name="permissions">Permissions required to access a resource.</param>
public class PermissionRequirement(
    params Permission[] permissions) : IAuthorizationRequirement
{
    /// <summary>Permissions required by this requirement.</summary>
    public Permission[] Permissions { get; set; } = permissions;
}