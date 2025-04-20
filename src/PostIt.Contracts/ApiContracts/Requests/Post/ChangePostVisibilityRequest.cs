using System.ComponentModel.DataAnnotations;
using PostIt.Domain.Enums;

namespace PostIt.Contracts.ApiContracts.Requests.Post;

public sealed record ChangePostVisibilityRequest(
    [Required] Guid PostId,
    [Required] Visibility Visibility);