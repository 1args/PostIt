namespace PostIt.Contracts.ApiContracts.Responses;

public record AuthResponse(
    string AccessToken,
    string RefreshToken);