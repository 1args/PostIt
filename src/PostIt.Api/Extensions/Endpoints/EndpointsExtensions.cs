using PostIt.Api.Endpoints;

namespace PostIt.Api.Extensions.Endpoints;

/// <summary>
/// Extension for registering Api endpoints.
/// </summary>
public static class EndpointsExtensions
{
    /// <summary>
    /// Registers Api endpoints.
    /// </summary>
    /// <returns>Router with added endpoints.</returns>
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapUserEndpoints();
        endpoints.MapPostEndpoints();
        endpoints.MapCommentEndpoints();
        
        return endpoints;
    }
}