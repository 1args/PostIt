using PostIt.Application.Contracts.Requests.Post;
using PostIt.Application.Contracts.Responses;

namespace PostIt.Application.Abstractions.Services;

public interface IPostService
{
    Task<Guid> CreatePostAsync(CreatePostRequest request, CancellationToken cancellationToken);

    public Task UpdatePostAsync(UpdatePostRequest request, CancellationToken cancellationToken);

    Task DeletePostAsync(Guid postId, CancellationToken cancellationToken);

    Task LikePostAsync(Guid postId, Guid authorId, CancellationToken cancellationToken);

    Task UnlikePostAsync(Guid postId, Guid authorId, CancellationToken cancellationToken);

    Task ViewPostAsync(Guid postId, CancellationToken cancellationToken);

    Task ChangeVisibilityAsync(ChangePostVisibilityRequest request, CancellationToken cancellationToken);

    Task<List<PostResponse>> GetPostsSortedByLikesAsync(Guid currentUserId, CancellationToken cancellationToken);

    Task<List<PostResponse>> GetPostsSortedByViewsAsync(Guid currentUserId, CancellationToken cancellationToken);
}