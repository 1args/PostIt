using PostIt.Contracts.ApiContracts.Responses;
using PostIt.Domain.Entities;

namespace PostIt.Contracts.Mappers;

public static class CommentMapper
{
    public static CommentResponse MapToPublic(this Comment comment) =>
        new CommentResponse(
            Id: comment.Id,
            Text: comment.Text.Value,
            AuthorId: comment.AuthorId,
            PostId: comment.PostId,
            CreatedAt: comment.CreatedAt,
            LikesCount: comment.Likes.Count);
}