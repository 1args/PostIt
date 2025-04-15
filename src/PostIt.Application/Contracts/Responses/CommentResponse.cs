namespace PostIt.Application.Contracts.Responses;

public record CommentResponse(
    Guid CommentId,
    string Text,
    Guid AuthorId,
    Guid PostId,
    DateTime CreatedAt,
    int LikesCount);