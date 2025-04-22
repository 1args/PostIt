using Microsoft.AspNetCore.Mvc;
using PostIt.Application.Abstractions.Services;
using PostIt.Contracts.ApiContracts.Requests.Comment;
using PostIt.Contracts.ApiContracts.Requests.User;

namespace PostIt.Api.Endpoints;

public static class UserEndpoints 
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/users").WithTags("Users");
        
        group.MapPost("/", CreateUserAsync);
        group.MapGet("/{id:guid}", GetUserByIdAsync).WithName(nameof(GetUserByIdAsync));
        group.MapDelete("{id:guid}", DeleteUserAsync).WithName(nameof(DeleteUserAsync));
        group.MapPut("{id:guid}/bio", UpdateUserBioAsync).WithName(nameof(UpdateUserBioAsync));
        
        return endpoints;
    }

    private static async Task<IResult> CreateUserAsync(
        [FromBody] CreateUserRequest request,
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        await userService.CreateUserAsync(request, cancellationToken);

        return Results.Created();
    }

    private static async Task<IResult> GetUserByIdAsync(
        [FromRoute] Guid id,
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByIdAsync(id, cancellationToken);

        return Results.Ok(user);
    }

    private static async Task<IResult> DeleteUserAsync(
        [FromRoute] Guid id,
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        await userService.DeleteUserAsync(id, cancellationToken);

        return Results.NoContent();
    }

    private static async Task<IResult> UpdateUserBioAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateUserBioRequest request,
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        await userService.UpdateUserBioAsync(id, request, cancellationToken);
        
        return Results.NoContent();
    }
}

