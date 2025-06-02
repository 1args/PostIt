namespace PostIt.Contracts.Responses;

/// <summary>
/// Represents a response with the required authentication information.
/// </summary>
public record AuthResponse(
    string AccessToken,
    string RefreshToken);