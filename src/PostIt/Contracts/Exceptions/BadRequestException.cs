namespace PostIt.Contracts.Exceptions;

/// <summary>
/// Thrown when a bad request is made by the client.
/// </summary>
public class BadRequestException(string message) : Exception(message);