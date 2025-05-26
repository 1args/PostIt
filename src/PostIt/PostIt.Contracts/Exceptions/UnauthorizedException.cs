namespace PostIt.Contracts.Exceptions;

/// <summary>
/// Thrown when a user is unauthorized.
/// </summary>
public sealed class UnauthorizedException(string message) : Exception(message);