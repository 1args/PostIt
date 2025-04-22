using PostIt.Domain.Entities;
using PostIt.Domain.Enums;

namespace PostIt.Contracts.ApiContracts.Responses;

public sealed record UserResponse(
    Guid Id,
    string Name,
    string Bio,
    Role Role,
    int PostsCount,
    int CommentsCount,
    DateTime CreatedAt);