namespace PostIt.Application.Exceptions;

public class InvalidTokenException : Exception
{
    public string? ParameterName { get; }
    
    public InvalidTokenException(string message) : base(message) { }
    
    public InvalidTokenException(string message, Exception innerException) 
        : base(message, innerException) { }
    
    public InvalidTokenException(string message, string paramName)
        : base(message)
        => ParameterName = paramName;
}