namespace PostIt.Contracts.ApiContracts.Requests.User;

/// <summary>
/// Represents a request to update the user's biography.
/// </summary>
public sealed record UpdateUserBioRequest(
    string Bio);