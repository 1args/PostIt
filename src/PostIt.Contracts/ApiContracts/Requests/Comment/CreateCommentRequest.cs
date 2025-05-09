namespace PostIt.Contracts.ApiContracts.Requests.Comment;

public record CreateCommentRequest(
    string Text,
    Guid PostId);