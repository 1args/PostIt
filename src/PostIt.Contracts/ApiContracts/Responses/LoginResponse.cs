namespace PostIt.Contracts.ApiContracts.Responses;

public record LoginResponse(
    string AccessToken,
    string RefreshToken);