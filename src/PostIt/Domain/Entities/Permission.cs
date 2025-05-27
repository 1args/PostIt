using PostIt.Domain.Primitives;

namespace PostIt.Domain.Entities;

/// <summary>
/// Represents a permission entity.
/// </summary>
public sealed class Permission : Entity<int>
{
    /// <summary><summary>Name of the permission.</summary></summary>
    public string Name { get; private set; }

    /// <summary>Collection of roles that have this permission.</summary>
    public ICollection<Role> Roles { get; private set; } = null!;

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private Permission() { }

    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private Permission(int id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>
    /// Factory method to create a new permission.
    /// </summary>
    /// <param name="id">Permission ID.</param>
    /// <param name="name">Name of permission.</param>
    /// <returns>A new instance of the <see cref="Permission"/> class.</returns>
    public static Permission Create(int id, string name) => new(id, name);
}