using PostIt.Contracts.Requests.User;
using PostIt.Contracts.Responses;

namespace PostIt.Application.Abstractions.Services;

/// <summary>
/// Provides functionality for managing user accounts.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves the current user details.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Response containing the user's details.</returns>
    Task<UserResponse> GetCurrentUserAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Retrieves the user details.
    /// </summary>
    /// <param name="userId">ID of the user whose details are to be retrieved.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Response containing the user's details.</returns>
    Task<UserResponse> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a user account by its ID.
    /// </summary>
    /// <param name="userId">ID of the user to be deleted.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the bio information of a user.
    /// </summary>
    /// <param name="userId">ID of the user whose bio will be updated.</param>
    /// <param name="request">Request containing the new bio information.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UpdateUserBioAsync(Guid userId, UpdateUserBioRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Allows a user to follow another user.
    /// </summary>
    /// <param name="followingId">ID of the user to be followed.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task FollowUserAsync(Guid followingId, CancellationToken cancellationToken);

    /// <summary>
    /// Allows a user to unfollow another user.
    /// </summary>
    /// <param name="followingId">ID of the user to be unfollowed.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UnfollowUserAsync(Guid followingId, CancellationToken cancellationToken);
}