using PostIt.Contracts.ApiContracts.Requests.Post;
using PostIt.Contracts.ApiContracts.Responses;

namespace PostIt.Application.Abstractions.Services;

public interface IPostService
{
    Task<Guid> CreatePostAsync(CreatePostRequest request, CancellationToken cancellationToken);

    Task UpdatePostAsync(Guid postId, UpdatePostRequest request, CancellationToken cancellationToken);

    Task DeletePostAsync(Guid postId, CancellationToken cancellationToken);

    Task LikePostAsync(Guid postId, Guid authorId, CancellationToken cancellationToken);

    Task UnlikePostAsync(Guid postId, Guid authorId, CancellationToken cancellationToken);

    Task ViewPostAsync(Guid postId, CancellationToken cancellationToken);

    Task ChangeVisibilityAsync(Guid postId, ChangePostVisibilityRequest request, CancellationToken cancellationToken);

    Task<List<PostResponse>> GetPostsSortedByLikesAsync(Guid currentUserId, CancellationToken cancellationToken);

    Task<List<PostResponse>> GetPostsSortedByViewsAsync(Guid currentUserId, CancellationToken cancellationToken);
}