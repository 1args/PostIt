using Microsoft.AspNetCore.Http;

namespace PostIt.Contracts.ApiContracts.Requests.User;

/// <summary>
/// Represents a request to upload a user avatar.
/// </summary>
/// <param name="Avatar"></param>
public record UploadAvatarRequest(
    IFormFile Avatar);