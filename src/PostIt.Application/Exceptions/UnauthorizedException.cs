namespace PostIt.Application.Exceptions;

/// <summary>
/// Thrown when a user is unauthorized.
/// </summary>
public class UnauthorizedException(string message) : Exception(message);