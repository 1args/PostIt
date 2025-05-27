namespace PostIt.Domain.Entities;

/// <summary>
/// Linking table for user-role many-to-many relationship.
/// </summary>
public class UserRole
{
    /// <summary>User identifier.</summary>
    public Guid UserId { get; private set; }
    
    /// <summary>Role identifier.</summary>
    public int RoleId { get; private set; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private UserRole() { }
    
    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private UserRole(Guid userId, int roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }

    /// <summary>
    /// Factory method to create a new user role.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="roleId">Role ID.</param>
    /// <returns>A new instance of the <see cref="UserRole"/> class.</returns>
    public static UserRole Create(Guid userId, int roleId) => new(userId, roleId);
}