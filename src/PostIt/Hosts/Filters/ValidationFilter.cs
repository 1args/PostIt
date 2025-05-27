using FluentValidation;

namespace PostIt.Hosts.Filters;

/// <summary>
/// Filter for validating requests to endpoints.
/// </summary>
/// <typeparam name="TRequest">Type of request to be validated.</typeparam>
public class ValidationFilter<TRequest>(IValidator<TRequest> validator) : IEndpointFilter
{
    /// <inheritdoc/>
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context, 
        EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<TRequest>().First();

        var validationResult = await validator.ValidateAsync(request, context.HttpContext.RequestAborted);

        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }
        
        return await next(context);
    }
}