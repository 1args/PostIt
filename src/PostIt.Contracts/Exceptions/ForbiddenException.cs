namespace PostIt.Contracts.Exceptions;

/// <summary>
/// Thrown when access is denied due to insufficient rights.
/// </summary>
public class ForbiddenException(string message) : Exception(message);