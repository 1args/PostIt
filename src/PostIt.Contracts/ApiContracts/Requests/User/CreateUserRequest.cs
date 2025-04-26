using PostIt.Domain.Enums;

namespace PostIt.Contracts.ApiContracts.Requests.User;

public sealed record CreateUserRequest(
    string Name,
    string Bio,
    string Email,
    string Password,
    Role Role);
