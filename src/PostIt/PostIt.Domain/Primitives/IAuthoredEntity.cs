namespace PostIt.Domain.Primitives;

/// <summary>
/// Represents an entity that has an author (a user who created it).
/// </summary>
public interface IAuthoredEntity
{
    /// <summary>Unique identifier of the entity.</summary>
    Guid Id { get; }
    
    /// <summary>Unique identifier of the user who authored the entity.</summary>
    Guid AuthorId { get; }
}