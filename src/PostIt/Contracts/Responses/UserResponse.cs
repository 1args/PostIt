using PostIt.Domain.Entities;

namespace PostIt.Contracts.Responses;

/// <summary>
/// Represents a response containing information about a user.
/// </summary>
public sealed record UserResponse(
    Guid Id,
    string Name,
    string Bio,
    ICollection<Role> Roles,
    int PostsCount,
    int CommentsCount,
    DateTime CreatedAt);