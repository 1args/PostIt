namespace PostIt.Application.Exceptions;

/// <summary>
/// Thrown on security-related issues.
/// </summary>
public class SecurityException(string message) : Exception(message);