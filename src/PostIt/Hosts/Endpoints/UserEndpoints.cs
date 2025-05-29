using Microsoft.AspNetCore.Mvc;
using PostIt.Api.Extensions.Endpoints;
using PostIt.Application.Abstractions.Services;
using PostIt.Contracts.Requests.User;
using PostIt.Domain.Enums;
using PostIt.Hosts.Extensions.Endpoints;

namespace PostIt.Hosts.Endpoints;

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
            .RequireAuthorization();

        group.MapPost("/refresh-token", RefreshTokenAsync)
            .RequireAuthorization();
        
        group.MapPatch("/{id:guid}/roles/restricted", RestrictUserAsync)
            .RequireAuthorization()
            .RequirePermissions(Permission.ManageRestrictedUsers);

        group.MapDelete("/{id:guid}/roles/restricted", UnrestrictUserAsync)
            .RequireAuthorization()
            .RequirePermissions(Permission.ManageRestrictedUsers);
        
        group.MapPatch("/{id:guid}/roles/moderator", AssignModeratorRoleAsync)
            .RequireAuthorization()
            .RequirePermissions(Permission.ManageUsers);
        
        group.MapDelete("/{id:guid}/roles/moderator", UnassignModeratorRoleAsync)
            .RequireAuthorization()
            .RequirePermissions(Permission.ManageUsers);

        group.MapPatch("/avatar", UploadAvatarAsync)
            .WithRequestValidation<UploadAvatarRequest>()
            .RequireAuthorization()
            .RequirePermissions(Permission.EditOwnProfile)
            .DisableAntiforgery();

        group.MapGet("/{id:guid}/avatar", GetAvatarAsync);

        group.MapPut("/{id:guid}/bio", UpdateUserBioAsync)
            .WithRequestValidation<UpdateUserBioRequest>()
            .RequireAuthorization()
            .RequirePermissions(Permission.EditOwnProfile);

        group.MapPost("/follow/{id:guid}", FollowUserAsync)
            .RequireAuthorization();
        
        group.MapPost("/unfollow/{id:guid}", UnFollowUserAsync)
            .RequireAuthorization();
        
        group.MapGet("/me", GetCurrentUserAsync)
            .RequireAuthorization();
        
        group.MapGet("me/avatar", GetCurrentUserAvatarAsync)
            .RequireAuthorization();
        
        group.MapGet("/{id:guid}", GetUserByIdAsync);
        
        group.MapDelete("/{id:guid}", DeleteUserAsync);
        
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
    /// Restricts a specific user.
    /// </summary>
    private static async Task<IResult> RestrictUserAsync(
        [FromRoute] Guid id,
        [FromServices] IUserAccountService userAccountService,
        CancellationToken cancellationToken)
    {
        await userAccountService.RestrictUserAsync(id, cancellationToken);

        return Results.Ok();
    }
    
    /// <summary>
    /// Removes restrictions for a specific user.
    /// </summary>
    private static async Task<IResult> UnrestrictUserAsync(
        [FromRoute] Guid id,
        [FromServices] IUserAccountService userAccountService,
        CancellationToken cancellationToken)
    {
        await userAccountService.UnrestrictUserAsync(id, cancellationToken);

        return Results.Ok();
    }
    
    /// <summary>
    /// Assigns the moderator role.
    /// </summary>
    private static async Task<IResult> AssignModeratorRoleAsync(
        [FromRoute] Guid id,
        [FromServices] IUserAccountService userAccountService,
        CancellationToken cancellationToken)
    {
        await userAccountService.AssignModeratorRoleAsync(id, cancellationToken);

        return Results.Ok();
    }
    
    /// <summary>
    /// Unassign the moderator role.
    /// </summary>
    private static async Task<IResult> UnassignModeratorRoleAsync(
        [FromRoute] Guid id,
        [FromServices] IUserAccountService userAccountService,
        CancellationToken cancellationToken)
    {
        await userAccountService.UnassignModeratorRoleAsync(id, cancellationToken);

        return Results.Ok();
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
    /// Allows a user to follow another user.
    /// </summary>
    private static async Task<IResult> FollowUserAsync(
        [FromRoute] Guid id,
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        await userService.FollowUserAsync(id, cancellationToken);
        
        return Results.NoContent();
    }
    
    /// <summary>
    /// Allows a user to unfollow another user.
    /// </summary>
    private static async Task<IResult> UnFollowUserAsync(
        [FromRoute] Guid id,
        [FromServices] IUserService userService,
        CancellationToken cancellationToken)
    {
        await userService.UnfollowUserAsync(id, cancellationToken);
        
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