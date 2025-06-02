using PostIt.Domain.Enums;

namespace PostIt.Contracts.Responses;

/// <summary>
/// Represents a response containing detailed information about a post.
/// </summary>
public record PostResponse(
    Guid Id,
    string Title,
    string Content,
    int Views,
    int Likes,
    int Comments,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool WasUpdated,
    Visibility Visibility,
    Guid AuthorId);
