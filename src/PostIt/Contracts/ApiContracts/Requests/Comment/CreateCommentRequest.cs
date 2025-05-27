namespace PostIt.Contracts.ApiContracts.Requests.Comment;

/// <summary>
/// Represents a request to create a new comment.
/// </summary>
public sealed record CreateCommentRequest(
    string Text,
    Guid PostId);