namespace PostIt.Contracts.Requests.User;

/// <summary>
/// Represents a request to register a new user.
/// </summary>
public sealed record CreateUserRequest(
    string Name,
    string Bio,
    string Email,
    string Password);
