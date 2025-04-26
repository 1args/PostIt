using Microsoft.AspNetCore.Mvc;
using PostIt.Api.Extensions.Endpoints;
using PostIt.Api.Filters;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Validators.User;
using PostIt.Contracts.ApiContracts.Requests.Comment;
using PostIt.Contracts.ApiContracts.Requests.User;

namespace PostIt.Api.Endpoints;

public static class UserEndpoints 
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/users").WithTags("Users");
        
        group.MapPost("/register", RegisterAsync)
            .WithRequestValidation<CreateUserRequest>();
        
        group.MapPost("/login", LoginAsync)
            .WithRequestValidation<LoginRequest>();
        
        group.MapPut("{id:guid}/bio", UpdateUserBioAsync)
            .WithName(nameof(UpdateUserBioAsync))
            .WithRequestValidation<UpdateUserBioRequest>();
        
        group.MapGet("/{id:guid}", GetUserByIdAsync)
            .WithName(nameof(GetUserByIdAsync));
        
        group.MapDelete("{id:guid}", DeleteUserAsync).
            WithName(nameof(DeleteUserAsync));
        
        return endpoints;
    }

    private static async Task<IResult> RegisterAsync(
        [FromBody] CreateUserRequest request,
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        await userService.RegisterAsync(request, cancellationToken);

        return Results.Created();
    }
    
    private static async Task<IResult> LoginAsync(
        [FromBody] LoginRequest request,
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        var data = await userService
            .LoginAsync(request, cancellationToken);

        return Results.Ok(data);
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
}

