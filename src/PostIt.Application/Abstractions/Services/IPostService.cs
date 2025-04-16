using PostIt.Application.Contracts.Requests.Post;
using PostIt.Application.Contracts.Responses;

namespace PostIt.Application.Abstractions.Services;

public interface IPostService
{
    Task<Guid> CreatePostAsync(CreatePostRequest request, CancellationToken cancellationToken = default);

    public Task UpdatePostAsync(UpdatePostRequest request, CancellationToken cancellationToken = default);

    Task DeletePostAsync(Guid postId, CancellationToken cancellationToken = default);

    Task LikePostAsync(Guid postId, Guid authorId, CancellationToken cancellationToken = default);

    Task UnlikePostAsync(Guid postId, Guid authorId, CancellationToken cancellationToken = default);

    Task ViewPostAsync(Guid postId, CancellationToken cancellationToken = default);

    Task ChangeVisibilityAsync(ChangePostVisibilityRequest request, CancellationToken cancellationToken = default);

    Task<List<PostResponse>> GetPostsSortedByLikesAsync(CancellationToken cancellationToken = default);

    Task<List<PostResponse>> GetPostsSortedByViewsAsync(CancellationToken cancellationToken = default);
}