using PostIt.Domain.Enums;

namespace PostIt.Contracts.Requests.Post;

/// <summary>
/// Represents a request to change the visibility of a post.
/// </summary>
public sealed record ChangePostVisibilityRequest(
    Visibility Visibility);