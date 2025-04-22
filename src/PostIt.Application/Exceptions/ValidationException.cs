namespace PostIt.Application.Exceptions;

public class ValidationException : Exception
{
    public string? ParameterName { get; }
    
    public ValidationException(string message) : base(message) { }
    
    public ValidationException(string message, Exception innerException) 
        : base(message, innerException) { }
    
    public ValidationException(string message, string paramName)
        : base(message)
        => ParameterName = paramName;
}