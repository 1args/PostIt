using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PostIt.Application.Abstractions.Auth.Authorization;

namespace PostIt.Infrastructure.Auth.Authorization;

/// <summary>
/// Handles authorization by validating whether a user has the required permissions.
/// </summary>
public class PermissionAuthorizationHandler(
    IServiceScopeFactory serviceScopeFactory) : AuthorizationHandler<PermissionRequirement>
{
    /// <inheritdoc/>
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var userIdClaim = context.User.Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return;
        }
        
        using var scope = serviceScopeFactory.CreateScope();
        
        var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

        var permissions = await permissionService.GetUserPermissionsAsync(userId);

        if (permissions.Intersect(requirement.Permissions).Any())
        {
            context.Succeed(requirement);
        }
    }
}
