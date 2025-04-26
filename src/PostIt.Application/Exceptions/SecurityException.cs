namespace PostIt.Application.Exceptions;

public class SecurityException : Exception
{
    public string? ParameterName { get; }
    
    public SecurityException(string message) : base(message) { }
    
    public SecurityException(string message, Exception innerException) 
        : base(message, innerException) { }
    
    public SecurityException(string message, string paramName)
        : base(message)
        => ParameterName = paramName;
}