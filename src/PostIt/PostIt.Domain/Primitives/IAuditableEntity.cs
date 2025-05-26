namespace PostIt.Domain.Primitives;

/// <summary>
/// Interface for entities that must have a creation date.
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// Date and time when the entity was created.
    /// </summary>
    DateTime CreatedAt { get; }
}