using PostIt.Api.Endpoints;
using PostIt.Domain.Enums;
using PostIt.Infrastructure.Auth.Authorization;

namespace PostIt.Api.Extensions.Endpoints;

/// <summary>
/// Extensions for Api endpoints.
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

    /// <summary>
    /// Adds a requirement for specific permissions to access an endpoint.
    /// </summary>
    /// <typeparam name="TBuilder">Type of endpoint convention builder.</typeparam>
    /// <param name="builder">Endpoint convention builder.</param>
    /// <param name="permissions">List of permissions required to access the endpoint.</param>
    /// <returns>Updated TBuilder with permission requirements.</returns>
    public static IEndpointConventionBuilder RequirePermissions<TBuilder>(
        this TBuilder builder,
        params Permission[] permissions) 
        where TBuilder : IEndpointConventionBuilder
    {
        return builder.RequireAuthorization(policy => 
            policy.AddRequirements(new PermissionRequirement(permissions)));
    }
}