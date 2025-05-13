namespace PostIt.Contracts.Exceptions;

/// <summary>
/// Thrown when a token is invalid.
/// </summary>
public sealed class InvalidTokenException(string message) : Exception(message);