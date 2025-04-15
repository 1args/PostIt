using System.ComponentModel.DataAnnotations;

namespace PostIt.Application.Contracts.Requests.Comment;

public record CreateCommentRequest(
    [Required] string Text,
    [Required] Guid AuthorId,
    [Required] Guid PostId);