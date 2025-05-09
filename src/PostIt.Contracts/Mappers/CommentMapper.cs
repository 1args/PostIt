using PostIt.Contracts.ApiContracts.Responses;
using PostIt.Domain.Entities;

namespace PostIt.Contracts.Mappers;

/// <summary>
/// Provides mapping functionality from <see cref="Comment"/> domain entity to <see cref="CommentResponse"/> DTO.
/// </summary>
public static class CommentMapper
{
    /// <summary>
    /// Maps a <see cref="Comment"/> entity to a publicly exposed <see cref="CommentResponse"/>.
    /// </summary>
    /// <param name="comment">Comment entity to map.</param>
    /// <returns><see cref="CommentResponse"/> containing public data about the comment.</returns>
    public static CommentResponse MapToPublic(this Comment comment) =>
        new CommentResponse(
            Id: comment.Id,
            Text: comment.Text.Value,
            AuthorId: comment.AuthorId,
            PostId: comment.PostId,
            CreatedAt: comment.CreatedAt,
            LikesCount: comment.Likes.Count);
}