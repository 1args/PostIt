using PostIt.Domain.Enums;

namespace PostIt.Contracts.ApiContracts.Responses;

/// <summary>
/// Represents a response containing information about a user.
/// </summary>
public sealed record UserResponse(
    Guid Id,
    string Name,
    string Bio,
    Role Role,
    int PostsCount,
    int CommentsCount,
    DateTime CreatedAt);