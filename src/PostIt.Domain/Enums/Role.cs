namespace PostIt.Domain.Enums;

/// <summary>
/// Represents the role assigned to a user within the system.
/// </summary>
public enum Role
{
    /// <summary>Standard user with basic privileges.</summary>
    User = 0,
    
    /// <summary>User with additional privileges to moderate content.</summary>
    Moderator = 1,
    
    /// <summary>User with full administrative access and permissions.</summary>
    Admin = 2,
}