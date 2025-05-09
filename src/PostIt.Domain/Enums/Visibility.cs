namespace PostIt.Domain.Enums;

/// <summary>
/// Indicates the level of visibility of the post.
/// </summary>
public enum Visibility
{
    /// <summary>The entity is visible to all users.</summary>
    Public = 0,
    
    /// <summary>The entity is visible only to the owner or specific users.</summary>
    Private = 1
}