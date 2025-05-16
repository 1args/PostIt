namespace PostIt.Domain.Entities;

/// <summary>
/// Linking table for role-permission many-to-many relationship.
/// </summary>
public class RolePermission
{
    /// <summary>Role identifier.</summary>
    public int RoleId { get; set; }

    /// <summary>Permission identifier.</summary>
    public int PermissionId { get; set; }
}