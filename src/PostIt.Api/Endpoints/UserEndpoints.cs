using Microsoft.AspNetCore.Mvc;
using PostIt.Api.Extensions.Endpoints;
using PostIt.Application.Abstractions.Services;
using PostIt.Contracts.ApiContracts.Requests.User;

namespace PostIt.Api.Endpoints;

/// <summary>
/// Provides API endpoints for user-related actions.
/// </summary>
public static class UserEndpoints 
{
    /// <summary>
    /// Maps all user-related endpoints to the application's route builder.
    /// </summary>
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
            .RequireAuthorization()
            .WithName(nameof(LogoutAsync));

        group.MapPost("/refresh-token", RefreshTokenAsync)
            .RequireAuthorization()
            .WithName(nameof(RefreshTokenAsync));

        group.MapPatch("/avatar", UploadAvatarAsync)
            .WithRequestValidation<UploadAvatarRequest>()
            .RequireAuthorization()
            .DisableAntiforgery()
            .WithName(nameof(UploadAvatarAsync));

        group.MapGet("{id:guid}/avatar", GetAvatarAsync)
            .WithName(nameof(GetAvatarAsync));

        group.MapPut("{id:guid}/bio", UpdateUserBioAsync)
            .WithRequestValidation<UpdateUserBioRequest>()
            .WithName(nameof(UpdateUserBioAsync));
        
        group.MapGet("/me", GetUserByIdAsync)
            .WithName(nameof(GetUserByIdAsync));
        
        group.MapGet("/{id:guid}", GetUserByIdAsync)
            .WithName(nameof(GetUserByIdAsync));
        
        group.MapDelete("{id:guid}", DeleteUserAsync).
            WithName(nameof(DeleteUserAsync));
        
        return endpoints;
    }

    /// <summary>
    /// Registers a new user with provided registration data.
    /// </summary>
    private static async Task<IResult> RegisterAsync(
        [FromBody] CreateUserRequest request,
        [FromServices] IUserAccountService userAccountService,
        CancellationToken cancellationToken)
    {
        await userAccountService.RegisterAsync(request, cancellationToken);

        return Results.Created();
    }

    /// <summary>
    /// Verifies user's email using provided user ID and verification token.
    /// </summary>
    private static async Task<IResult> VerifyEmailAsync(
        [FromRoute] Guid userId,
        [FromRoute] Guid token,
        [FromServices] IUserAccountService userAccountService,
        CancellationToken cancellationToken)
    {
        var success = await userAccountService.VerifyEmailAsync(userId, token, cancellationToken);
        
        return success
            ? Results.Ok("Email verified. You can close this window.")
            : Results.BadRequest("Validation token is expired.");
    }
    
    /// <summary>
    /// Logs in a user with credentials and returns tokens.
    /// </summary>
    private static async Task<IResult> LoginAsync(
        [FromBody] LoginRequest request,
        [FromServices] IUserAccountService userAccountService,
        CancellationToken cancellationToken)
    {
        var data = await userAccountService
            .LoginAsync(request, cancellationToken);

        return Results.Ok(data);
    }

    /// <summary>
    /// Logs out the current authenticated user.
    /// </summary>
    private static async Task<IResult> LogoutAsync(
        [FromServices] IUserAccountService userAccountService,
        CancellationToken cancellationToken)
    {
        await userAccountService.LogoutAsync(cancellationToken);
        
        return Results.SignOut();
    }

    /// <summary>
    /// Refreshes the access token for the authenticated user.
    /// </summary>
    private static async Task<IResult> RefreshTokenAsync(
        [FromServices] IUserAccountService userAccountService,
        CancellationToken cancellationToken)
    {
        var data = await userAccountService.RefreshToken(cancellationToken);
        
        return Results.Ok(data);
    }
    
    /// <summary>
    /// Uploads a new avatar image for the authenticated user.
    /// </summary>
    private static async Task<IResult> UploadAvatarAsync(
        [FromForm] UploadAvatarRequest request,
        [FromServices] IAvatarManagementService avatarManagementService,
        CancellationToken cancellationToken)
    {
        await using var stream = new MemoryStream();
        await request.Avatar.CopyToAsync(stream, cancellationToken);

        await avatarManagementService.UploadAvatarAsync(stream.ToArray(), cancellationToken);

        return Results.Created();
    }
    
    /// <summary>
    /// Downloads the avatar image for the authenticated user.
    /// </summary>
    private static async Task<IResult> GetCurrentUserAvatarAsync(
        [FromServices] IAvatarManagementService avatarManagementService,
        CancellationToken cancellationToken)
    {
        var avatar = await avatarManagementService.GetCurrentUserAvatarAsync(cancellationToken);

        return Results.File(avatar.ToArray(), "image/webp");
    }

    /// <summary>
    /// Downloads the avatar image for the user by their ID.
    /// </summary>
    private static async Task<IResult> GetAvatarAsync(
        [FromRoute] Guid id,
        [FromServices] IAvatarManagementService avatarManagementService,
        CancellationToken cancellationToken)
    {
        var avatar = await avatarManagementService.GetAvatarAsync(id, cancellationToken);

        return Results.File(avatar.ToArray(), "image/webp");
    }

    /// <summary>
    /// Updates the biography information for a specific user.
    /// </summary>
    private static async Task<IResult> UpdateUserBioAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateUserBioRequest request,
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        await userService.UpdateUserBioAsync(id, request, cancellationToken);
        
        return Results.NoContent();
    }
    
    /// <summary>
    /// Retrieves user information by their ID.
    /// </summary>
    private static async Task<IResult> GetCurrentUserAsync(
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        var user = await userService.GetCurrentUserAsync(cancellationToken);

        return Results.Ok(user);
    }
    
    /// <summary>
    /// Retrieves user information by their ID.
    /// </summary>
    private static async Task<IResult> GetUserByIdAsync(
        [FromRoute] Guid id,
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByIdAsync(id, cancellationToken);

        return Results.Ok(user);
    }

    /// <summary>
    /// Deletes a user account by their ID.
    /// </summary>
    private static async Task<IResult> DeleteUserAsync(
        [FromRoute] Guid id,
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        await userService.DeleteUserAsync(id, cancellationToken);

        return Results.NoContent();
    }
}