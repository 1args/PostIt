using PostIt.Domain.Entities;
using PostIt.Domain.Enums;

namespace PostIt.Application.Contracts.Responses;

public sealed record UserResponse(
    Guid UserId,
    string Name,
    string Bio,
    Role Role,
    IReadOnlyList<Post> Posts,
    IReadOnlyList<Comment> Comments,
    DateTime CreatedAt);