using PostIt.Domain.Enums;

namespace PostIt.Application.Contracts.Responses;

public record PostResponse(
    Guid Id,
    string Title,
    string Content,
    int LikesCount,
    int CommentsCount,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    bool WasUpdated,
    Visibility Visibility,
    Guid AuthorId);
