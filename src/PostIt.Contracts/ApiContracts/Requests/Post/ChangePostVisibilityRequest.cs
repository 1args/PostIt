using PostIt.Domain.Enums;

namespace PostIt.Contracts.ApiContracts.Requests.Post;

public sealed record ChangePostVisibilityRequest(
    Visibility Visibility);