using PostIt.Contracts.ApiContracts.Responses;
using PostIt.Domain.Entities;

namespace PostIt.Contracts.Mappers;

public static class UserMapper
{
    public static UserResponse MapToPublic(this User user) =>
        new UserResponse(
            Id: user.Id,
            Name: user.Name.Value,
            Bio: user.Bio.Value,
            Role: user.Role,
            PostCount: user.PostsCount,
            Posts: user.Posts,
            Comments: user.Comments,
            CreatedAt: user.CreatedAt);
}