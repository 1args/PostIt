using Microsoft.AspNetCore.Authorization;
using PostIt.Domain.Enums;

namespace PostIt.Infrastructure.Auth.Authorization;

/// <summary>
/// Represents a permission requirement used in authorization policies.
/// </summary>
/// <param name="permissions">Permissions required to access a resource.</param>
public class PermissionRequirement(
    params Permission[] permissions) : IAuthorizationRequirement
{
    public Permission[] Permissions { get; set; } = permissions;
}