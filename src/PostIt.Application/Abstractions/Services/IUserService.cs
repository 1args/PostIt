using PostIt.Contracts.ApiContracts.Requests.User;
using PostIt.Contracts.ApiContracts.Responses;

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
    /// Restricts a specific user.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task RestrictUserAsync(Guid userId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Removes restrictions for a specific user.
    /// </summary>
    /// <param name="userId">User ID.></param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UnrestrictUserAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Assigns the moderator role.
    /// </summary>
    /// <param name="userId">User ID.></param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task AssignModeratorRoleAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Unassign the moderator role.
    /// </summary>
    /// <param name="userId">User ID.></param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UnassignModeratorRoleAsync(Guid userId, CancellationToken cancellationToken);
}