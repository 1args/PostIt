using PostIt.Domain.Primitives;

namespace PostIt.Domain.Entities;

/// <summary>
/// Represents a role entity.
/// </summary>
public sealed class Role : Entity<int>
{
    /// <summary>Name of the role.</summary>
    public string Name { get; private set; }

    /// <summary>Collection of permissions assigned to the role.</summary>
    public IReadOnlyCollection<Permission> Permissions { get; private set; } = null!;
    
    /// <summary>Collection of users who have this role.</summary>
    public IReadOnlyCollection<User> Users { get; private set; } = [];

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private Role() { }
    
    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private Role(int id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>
    /// Factory method to create a new role.
    /// </summary>
    /// <param name="id">Role ID.</param>
    /// <param name="name">Name of role.</param>
    /// <returns>A new instance of the <see cref="Role"/> class.</returns>
    public static Role Create(int id, string name) => new(id, name);
}