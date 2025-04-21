using PostIt.Api.Endpoints;

namespace PostIt.Api.Extensions;

public static class EndpointsRegister
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapUserEndpoints();
        endpoints.MapPostEndpoints();
        endpoints.MapCommentEndpoints();
        
        return endpoints;
    }
}