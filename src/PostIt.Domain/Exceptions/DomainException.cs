using System.Runtime.Serialization;

namespace PostIt.Domain.Exceptions;

public class DomainException : Exception
{
    public string? ParameterName { get; }

    public DomainException(string message, string paramName)
        : base(message)
        => ParameterName = paramName;
    
    public DomainException(string message) : base(message) { }
    
    public DomainException(string message, Exception innerException) 
        : base(message, innerException) { }
    
}