namespace PostIt.Contracts.ApiContracts.Responses;

public record LoginResponse(
    Guid UserId,
    string AccessToken,
    string RefreshToken);