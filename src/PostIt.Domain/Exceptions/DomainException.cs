namespace PostIt.Domain.Exceptions;

public class DomainException : Exception
{
    public string Code { get; }
    
    public int? StatusCode { get; }

    public DomainException(string message, string? code = null, int? statusCode = null) 
        : base(message)
    {
        Code = code ?? "Domain.Error";
        StatusCode = statusCode;
    }

    public DomainException(string message, Exception innerException, string? code = null)
        : base(message, innerException)
    {
        Code = code ?? "Domain.Error";
    }
}