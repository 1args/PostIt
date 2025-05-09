using PostIt.Contracts.ApiContracts.Responses;
using PostIt.Domain.Entities;

namespace PostIt.Contracts.Mappers;

/// <summary>
/// Provides mapping functionality from <see cref="Post"/> domain entity to <see cref="PostResponse"/> DTO.
/// </summary>
public static class PostMapper
{
    /// <summary>
    /// Maps a <see cref="Post"/> entity to a publicly exposed <see cref="PostResponse"/>.
    /// </summary>
    /// <param name="post">Post entity to map.</param>
    /// <param name="currentUserId">ID of the current user, used to determine if the post is liked.</param>
    /// <returns><see cref="PostResponse"/> containing public data about the post.</returns>
    public static PostResponse MapToPublic(this Post post, Guid currentUserId)
    {
        var isLiked = post.Likes.Any(p => p.AuthorId == currentUserId);

        return new PostResponse(
            Id: post.Id,
            Title: post.Title.Value,
            Content: post.Content.Value,
            LikesCount: post.LikesCount,
            CommentsCount: post.Comments.Count,
            CreatedAt: post.CreatedAt,
            UpdatedAt: post.UpdatedAt,
            WasUpdated: post.WasUpdated,
            Visibility: post.Visibility,
            AuthorId: post.AuthorId,
            IsLiked: isLiked);
    }
}