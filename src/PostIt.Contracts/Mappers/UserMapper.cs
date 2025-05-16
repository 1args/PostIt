using PostIt.Contracts.ApiContracts.Responses;
using PostIt.Domain.Entities;

namespace PostIt.Contracts.Mappers;

/// <summary>
/// Provides mapping functionality from <see cref="User"/> domain entity to <see cref="UserResponse"/> DTO.
/// </summary>
public static class UserMapper
{
    /// <summary>
    /// Maps a <see cref="User"/> entity to a publicly exposed <see cref="UserResponse"/>.
    /// </summary>
    /// <param name="user">User entity to map.</param>
    /// <returns><see cref="UserResponse"/> containing public data about the user.</returns>
    public static UserResponse MapToPublic(this User user) =>
        new UserResponse(
            Id: user.Id,
            Name: user.Name.Value,
            Bio: user.Bio.Value,
            Roles: user.Roles.ToList(),
            PostsCount: user.PostsCount,
            CommentsCount: user.CommentsCount,
            CreatedAt: user.CreatedAt);
}