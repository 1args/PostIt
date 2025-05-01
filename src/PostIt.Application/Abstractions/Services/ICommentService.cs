using PostIt.Contracts.ApiContracts.Requests.Comment;
using PostIt.Contracts.ApiContracts.Responses;

namespace PostIt.Application.Abstractions.Services;

public interface ICommentService
{
    Task<Guid> CreateCommentAsync(CreateCommentRequest request, CancellationToken cancellationToken);

    Task DeleteCommentAsync(Guid commentId, CancellationToken cancellationToken);

    Task LikeCommentAsync(Guid commentId, CancellationToken cancellationToken);

    Task UnlikeCommentAsync(Guid commentId, CancellationToken cancellationToken);

    Task<List<CommentResponse>> GetCommentsByPostAsync(Guid postId, CancellationToken cancellationToken);
}