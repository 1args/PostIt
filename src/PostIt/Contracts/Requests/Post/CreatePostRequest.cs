using PostIt.Domain.Enums;

namespace PostIt.Contracts.Requests.Post;

/// <summary>
/// Represents a request to create a new post.
/// </summary>
public sealed record CreatePostRequest(
    string Title,
    string Content,
    Visibility Visibility = Visibility.Public);