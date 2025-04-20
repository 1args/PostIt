using System.ComponentModel.DataAnnotations;

namespace PostIt.Contracts.ApiContracts.Requests.Comment;

public record CreateCommentRequest(
    [Required] string Text,
    [Required] Guid AuthorId,
    [Required] Guid PostId);