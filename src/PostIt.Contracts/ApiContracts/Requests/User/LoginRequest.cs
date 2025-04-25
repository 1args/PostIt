namespace PostIt.Contracts.ApiContracts.Requests.User;

public record LoginRequest(
    string Email,
    string Password);