namespace PostIt.Contracts.Requests.User;

/// <summary>
/// Represents a request to log in a user.
/// </summary>
public sealed record LoginRequest(
    string Email,
    string Password);