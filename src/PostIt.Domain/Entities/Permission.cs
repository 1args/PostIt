using PostIt.Domain.Primitives;

namespace PostIt.Domain.Entities;

/// <summary>
/// Represents a permission entity.
/// </summary>
public class Permission : Entity<int>
{
    /// <summary><summary>Name of the permission.</summary></summary>
    public string Name { get; set; }
    
    /// <summary>Collection of roles that have this permission.</summary>
    public IReadOnlyCollection<Role> Roles { get; private set; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    public Permission() { }
}