namespace PostIt.Domain.Enums;

/// <summary>
/// Represents the role assigned to a user within the system.
/// </summary>
public enum Role
{
    /// <summary>User with restricted access (in other words, blocked).</summary>
    Restricted = 1,
    
    /// <summary>Standard user with basic privileges.</summary>
    User = 2,
    
    /// <summary>User with additional privileges to moderate content.</summary>
    Moderator = 3,
    
    /// <summary>User with full administrative access and permissions.</summary>
    Admin = 4,
}