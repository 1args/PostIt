using Microsoft.Extensions.Logging;
using PostIt.Application.Abstractions.Auth.Authorization;
using PostIt.Application.Abstractions.Utilities;
using PostIt.Contracts.Exceptions;
using PostIt.Domain.Enums;
using PostIt.Domain.Primitives;

namespace PostIt.Application.Utilities;

/// <inheritdoc/>
public class PermissionChecker<TEntity>(
    IPermissionService permissionService,
    ILogger<PermissionChecker<TEntity>> logger) 
    : IPermissionChecker<TEntity> where TEntity : class, IAuthoredEntity 
{
    /// <inheritdoc/>
    public async Task CheckPermissionsAsync(
        TEntity entity,
        Guid userId,
        Permission ownPermission,
        Permission moderatorPermission,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var permissions = await permissionService.GetUserPermissionsAsync(userId);
        
        if (!permissions.Contains(moderatorPermission))
        {
            if (!permissions.Contains(ownPermission) || entity.AuthorId != userId)
            {
                logger.LogWarning(
                    "User `{UserId}` attempted to perform an unauthorized action on {EntityType} `{EntityId}`.",
                    userId,
                    typeof(TEntity).Name, entity.Id);
                
                throw new UnauthorizedException(
                    $"You are not authorized to perform this action on the {typeof(TEntity).Name.ToLower()}.");
            }
        }
        else
        {
            logger.LogInformation(
                "User `{UserId}` with permission `{Permission}` is performing action on {EntityType} `{EntityId}`.",
                userId,
                moderatorPermission.ToString(),
                typeof(TEntity).Name,
                entity.Id);
        }
    }
}