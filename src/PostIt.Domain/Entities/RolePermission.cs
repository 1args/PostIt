namespace PostIt.Domain.Entities;

/// <summary>
/// Linking table for role-permission many-to-many relationship.
/// </summary>
public class RolePermission
{
    /// <summary>Role identifier.</summary>
    public int RoleId { get; private set; }

    /// <summary>Permission identifier.</summary>
    public int PermissionId { get; private set; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private RolePermission() { }
    
    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private RolePermission(int roleId, int permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }

    /// <summary>
    /// Factory method to create a new role permission.
    /// </summary>
    /// <param name="roleId">Role ID.</param>
    /// <param name="permissionId">Permission ID.</param>
    /// <returns>A new instance of the <see cref="RolePermission"/> class.</returns>
    public static RolePermission Create(int roleId, int permissionId) => new(roleId, permissionId);
}