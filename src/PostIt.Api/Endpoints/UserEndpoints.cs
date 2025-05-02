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

        group.MapGet("/verify-email/{userId:guid}/{token:guid}", VerifyEmailAsync)
            .WithName(nameof(VerifyEmailAsync));

        group.MapPost("/login", LoginAsync)
            .WithRequestValidation<LoginRequest>();

        group.MapPost("/logout", LogoutAsync)
            .WithName(nameof(LogoutAsync))
            .RequireAuthorization();

        group.MapPost("/refresh-token", RefreshTokenAsync)
            .WithName(nameof(RefreshTokenAsync))
            .RequireAuthorization();

        group.MapPatch("{id:guid}/avatar", UploadAvatarAsync)
            .WithName(nameof(UploadAvatarAsync))
            .RequireAuthorization();
        
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

    private static async Task<IResult> VerifyEmailAsync(
        [FromRoute] Guid userId,
        [FromRoute] Guid token,
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        var success = await userService.VerifyEmailAsync(userId, token, cancellationToken);
        
        return success
            ? Results.Ok("Email verified. You can close this window.")
            : Results.BadRequest("Validation token is expired.");
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

    private static async Task<IResult> LogoutAsync(
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        await userService.LogoutAsync(cancellationToken);
        
        return Results.NoContent();
    }

    private static async Task<IResult> RefreshTokenAsync(
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        var data = await userService.RefreshToken(cancellationToken);
        
        return Results.Ok(data);
    }
    
    private static async Task UploadAvatarAsync(
        [FromRoute] Guid id,
        [FromForm] IFormFile avatar,
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        await using var stream = new MemoryStream();
        await avatar.CopyToAsync(stream, cancellationToken);

        await userService.UploadAvatarAsync(stream.ToArray(), cancellationToken);
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

