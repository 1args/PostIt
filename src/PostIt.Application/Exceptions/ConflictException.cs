namespace PostIt.Application.Exceptions;

public class ConflictException : Exception
{
    public string? ParameterName { get; }
    
    public ConflictException(string message) : base(message) { }
    
    public ConflictException(string message, Exception innerException) 
        : base(message, innerException) { }
    
    public ConflictException(string message, string paramName)
        : base(message)
        => ParameterName = paramName;
}