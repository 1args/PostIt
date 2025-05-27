namespace PostIt.Contracts.Exceptions;

/// <summary>
/// Thrown when the Api fails and informs what went wrong for more specifics.
/// </summary>
public class ReadableException(string message) : Exception(message);