namespace PostIt.Contracts.Exceptions;

/// <summary>
/// Thrown when a token has expired.
/// </summary>
public sealed class TokenExpiredException(string message) : Exception(message);