namespace PostIt.Domain.Enums;

/// <summary>
/// Represents the permissions available in the system.
/// </summary>
public enum Permission
{
    /// <summary>No permission.</summary>
    None = 1,

    /// <summary>Permission to edit own profile.</summary>
    EditOwnProfile = 2,
    
    /// <summary>Permission to create a post.</summary>
    CreatePost = 3,

    /// <summary>Permission to edit own post.</summary>
    EditOwnPost = 4,

    /// <summary>Permission to delete own post.</summary>
    DeleteOwnPost = 5,

    /// <summary>Permission to edit any user's post.</summary>
    EditAnyPost = 6,

    /// <summary>Permission to delete any user's post.</summary>
    DeleteAnyPost = 7,

    /// <summary>Permission to create a comment.</summary>
    CreateComment = 8,

    /// <summary>Permission to delete own comment.</summary>
    DeleteOwnComment = 9,

    /// <summary>Permission to delete any user's comment.</summary>
    DeleteAnyComment = 10,

    /// <summary>Permission to like or dislike content.</summary>
    LikeDislike = 11,

    /// <summary>Permission to manage users with restricted access.</summary>
    ManageRestrictedUsers = 12,

    /// <summary>Permission to manage all users in the system.</summary>
    ManageUsers = 13
}