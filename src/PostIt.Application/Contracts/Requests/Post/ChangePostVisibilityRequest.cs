using System.ComponentModel.DataAnnotations;
using PostIt.Domain.Enums;

namespace PostIt.Application.Contracts.Requests.Post;

public sealed record ChangePostVisibilityRequest(
    [Required] Guid PostId,
    [Required] Visibility Visibility);