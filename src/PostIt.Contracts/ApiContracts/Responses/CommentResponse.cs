namespace PostIt.Contracts.ApiContracts.Responses;

public record CommentResponse(
    Guid Id,
    string Text,
    Guid AuthorId,
    Guid PostId,
    DateTime CreatedAt,
    int LikesCount);