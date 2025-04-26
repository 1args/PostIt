using PostIt.Domain.Enums;

namespace PostIt.Contracts.ApiContracts.Requests.Post;

public sealed record CreatePostRequest(
    string Title,
    string Content,
    Guid AuthorId,
    Visibility Visibility = Visibility.Public);