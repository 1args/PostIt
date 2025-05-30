using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PostIt.Contracts.Exceptions;
using PostIt.Domain.Exceptions;

namespace PostIt.Hosts.ErrorHandling;

/// <summary>
/// Represents a global exception handler that catches and processes exceptions
/// thrown during the request lifecycle, and returns standardized Problem Details responses.
/// </summary>
/// <param name="logger"> Logger used to record exception details.</param>
/// <param name="detailsService">Service used to write Problem Details responses.</param>
public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService detailsService) : IExceptionHandler
{
    /// <inheritdoc/>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception, 
        CancellationToken cancellationToken)
    {
        logger.LogError(
            "Exception occured while processing the {Method} request to path `{Path}` with message `{Message}`",
            httpContext.Request.Method,
            httpContext.Request.Path,
            exception.Message);

        var (statusCode, title, detail) = exception switch
        {
            DomainException => (StatusCodes.Status400BadRequest, "Bad Request", exception.Message),
            NotFoundException => (StatusCodes.Status404NotFound, "Not Found", exception.Message),
            ConflictException => (StatusCodes.Status409Conflict, "Conflict", exception.Message),
            UnauthorizedException => (StatusCodes.Status401Unauthorized, "Unauthorized", exception.Message),
            ForbiddenException => (StatusCodes.Status403Forbidden, "Forbidden", exception.Message),
            BadRequestException => (StatusCodes.Status400BadRequest, "Bad Request", exception.Message),
            ReadableException => (StatusCodes.Status500InternalServerError, "Internal Server Error", exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error",
                "An unexpected error occurred. Please try again later.")
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = GetErrorType(statusCode),
            Detail = detail
        };
        
        httpContext.Response.StatusCode = statusCode;

        return await detailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails
            });
    }
    
    private static string GetErrorType(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
        StatusCodes.Status404NotFound => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
        StatusCodes.Status409Conflict => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
        StatusCodes.Status401Unauthorized => "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
        StatusCodes.Status403Forbidden => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
        _ => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
    };
}