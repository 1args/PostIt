using PostIt.Domain.Enums;

namespace PostIt.Contracts.ApiContracts.Requests.User;

/// <summary>
/// Represents a request to register a new user.
/// </summary>
public sealed record CreateUserRequest(
    string Name,
    string Bio,
    string Email,
    string Password);
