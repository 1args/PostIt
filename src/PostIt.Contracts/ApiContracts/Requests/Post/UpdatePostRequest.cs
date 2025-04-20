using System.ComponentModel.DataAnnotations;

namespace PostIt.Contracts.ApiContracts.Requests.Post;

public record UpdatePostRequest(
    [Required] Guid PostId,
    [Required] string Title,
    [Required] string Content);