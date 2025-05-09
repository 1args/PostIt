using PostIt.Api.Endpoints;

namespace PostIt.Api.Extensions.Endpoints;

public static class EndpointsExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapUserEndpoints();
        endpoints.MapPostEndpoints();
        endpoints.MapCommentEndpoints();
        
        return endpoints;
    }
}