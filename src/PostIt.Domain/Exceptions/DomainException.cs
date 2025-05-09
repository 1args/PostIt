namespace PostIt.Domain.Exceptions;

/// <summary>
/// Special domain-level exception for all situations.
/// </summary>
public sealed class DomainException(string message) : Exception(message);