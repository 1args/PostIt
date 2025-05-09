namespace PostIt.Contracts.ApiContracts.Responses;

/// <summary>
/// Represents a response containing detailed information about a comment.
/// </summary>
public record CommentResponse(
    Guid Id,
    string Text,
    Guid AuthorId,
    Guid PostId,
    DateTime CreatedAt,
    int LikesCount);