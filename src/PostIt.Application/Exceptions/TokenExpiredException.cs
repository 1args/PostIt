namespace PostIt.Application.Exceptions;

public class TokenExpiredException : Exception
{
    public string? ParameterName { get; }
    
    public TokenExpiredException(string message) : base(message) { }
    
    public TokenExpiredException(string message, Exception innerException) 
        : base(message, innerException) { }
    
    public TokenExpiredException(string message, string paramName)
        : base(message)
        => ParameterName = paramName;
}
