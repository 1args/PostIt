namespace PostIt.Contracts.Requests.Post;

/// <summary>
/// Represents a request to update an existing post.
/// </summary>
public record UpdatePostRequest(
    string Title,
    string Content);