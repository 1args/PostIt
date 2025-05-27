namespace PostIt.Contracts.Exceptions;

/// <summary>
/// Thrown when a conflict occurs.
/// </summary>
public sealed class ConflictException(string message) : Exception(message);
