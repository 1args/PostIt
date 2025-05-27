using PostIt.Domain.Enums;
using PostIt.Domain.Primitives;

namespace PostIt.Application.Abstractions.Utilities;

/// <summary>
/// Checks whether a user has the necessary permissions to perform an action.
/// </summary>
/// <typeparam name="TEntity">Type of the entity. Must implement <see cref="IAuthoredEntity"/>.</typeparam>
public interface IPermissionChecker<in TEntity> where TEntity : class, IAuthoredEntity
{
    /// <summary>
    /// Validates if the specified user has either the "own" or "any" permission to perform an action on the given entity.
    /// </summary>
    /// <param name="entity">Entity on which the action is attempted.</param>
    /// <param name="userId">ID of the user attempting the action.</param>
    /// <param name="ownPermission">Defining a user's own access</param>
    /// <param name="moderatorPermission">Defining moderator access</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task CheckPermissionsAsync(TEntity entity, Guid userId, Permission ownPermission, Permission moderatorPermission,
        CancellationToken cancellationToken);
}