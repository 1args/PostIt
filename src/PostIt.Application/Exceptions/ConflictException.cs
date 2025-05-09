namespace PostIt.Application.Exceptions;

/// <summary>
/// Thrown when a conflict occurs.
/// </summary>
public class ConflictException(string message) : Exception(message);
