namespace PostIt.Application.Exceptions;

/// <summary>
/// Thrown when a token has expired.
/// </summary>
public class TokenExpiredException(string message) : Exception(message);