namespace PostIt.Application.Exceptions;

public class UnauthorizedException : Exception
{
    public string? ParameterName { get; }
    
    public UnauthorizedException(string message) : base(message) { }
    
    public UnauthorizedException(string message, Exception innerException) 
        : base(message, innerException) { }
    
    public UnauthorizedException(string message, string paramName)
        : base(message)
        => ParameterName = paramName;
}