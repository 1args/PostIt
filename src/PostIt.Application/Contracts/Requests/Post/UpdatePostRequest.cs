using System.ComponentModel.DataAnnotations;

namespace PostIt.Application.Contracts.Requests.Post;

public record UpdatePostRequest(
    [Required] Guid PostId,
    [Required] string Title,
    [Required] string Content);