namespace PostIt.Application.Exceptions;

public class NotFoundException : Exception
{
    public string? ParameterName { get; }
    
    public NotFoundException(string message) : base(message) { }
    
    public NotFoundException(string message, Exception innerException) 
        : base(message, innerException) { }
    
    public NotFoundException(string message, string paramName)
        : base(message)
        => ParameterName = paramName;
}