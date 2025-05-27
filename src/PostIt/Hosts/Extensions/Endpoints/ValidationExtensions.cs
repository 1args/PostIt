using PostIt.Hosts.Filters;

namespace PostIt.Hosts.Extensions.Endpoints;

/// <summary>
/// Extension to add request validation to endpoints.
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    /// Adds a request validation filter for the endpoint.
    /// </summary>
    /// <typeparam name="TRequest">The type of request model to be validated.</typeparam>
    /// <returns>Route building object with validation filter.</returns>
    public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
    {
        return builder
            .AddEndpointFilter<ValidationFilter<TRequest>>()
            .ProducesValidationProblem();
    }
}