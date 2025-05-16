using PostIt.Domain.Primitives;

namespace PostIt.Domain.Entities;

/// <summary>
/// Represents a role entity.
/// </summary>
public class Role : Entity<int>
{
    /// <summary>Name of the role.</summary>
    public string Name { get; set; }
    
    /// <summary>Collection of permissions assigned to the role.</summary>
    public IReadOnlyCollection<Permission> Permissions { get; private set; }
    
    /// <summary>Collection of users who have this role.</summary>
    public IReadOnlyCollection<User> Users { get; private set; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    public Role() { }
}