using System.ComponentModel.DataAnnotations;
using PostIt.Domain.Enums;

namespace PostIt.Application.Contracts.Requests.Post;

public sealed record CreatePostRequest(
    [Required] string Title,
    [Required] string Content,
    [Required] Guid AuthorId,
    Visibility Visibility = Visibility.Public);