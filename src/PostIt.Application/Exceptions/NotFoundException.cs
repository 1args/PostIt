namespace PostIt.Application.Exceptions;

/// <summary>
/// Thrown when a resource is not found.
/// </summary>
public class NotFoundException(string message) : Exception(message);