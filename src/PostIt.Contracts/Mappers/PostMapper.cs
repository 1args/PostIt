using PostIt.Contracts.ApiContracts.Responses;
using PostIt.Domain.Entities;

namespace PostIt.Contracts.Mappers;

public static class PostMapper
{
    public static PostResponse MapToPublic(this Post post, Guid currentUserId)
    {
        var isLiked = post.Likes.Any(p => p.AuthorId == currentUserId);

        return new PostResponse(
            Id: post.Id,
            Title: post.Title.Value,
            Content: post.Content.Value,
            LikesCount: post.Likes.Count,
            CommentsCount: post.Comments.Count,
            CreatedAt: post.CreatedAt,
            UpdatedAt: post.UpdatedAt,
            WasUpdated: post.WasUpdated,
            Visibility: post.Visibility,
            AuthorId: post.AuthorId,
            IsLiked: isLiked);
    }
}