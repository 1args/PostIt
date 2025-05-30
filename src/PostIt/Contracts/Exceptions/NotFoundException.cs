namespace PostIt.Contracts.Exceptions;

/// <summary>
/// Thrown when a resource is not found.
/// </summary>
public sealed class NotFoundException(string message) : Exception(message);