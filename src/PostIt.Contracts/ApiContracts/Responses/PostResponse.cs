using PostIt.Domain.Enums;

namespace PostIt.Contracts.ApiContracts.Responses;

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
    Guid AuthorId,
    bool IsLiked);
