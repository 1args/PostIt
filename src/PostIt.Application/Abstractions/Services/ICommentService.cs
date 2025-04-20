using PostIt.Contracts.ApiContracts.Requests.Comment;
using PostIt.Contracts.ApiContracts.Responses;

namespace PostIt.Application.Abstractions.Services;

public interface ICommentService
{
    Task<Guid> CreateComment(CreateCommentRequest request, CancellationToken cancellationToken);

    Task DeleteComment(Guid commentId, CancellationToken cancellationToken);

    Task LikeCommentAsync(Guid commentId, Guid authorId, CancellationToken cancellationToken);

    Task UnlikeCommentAsync(Guid commentId, Guid authorId, CancellationToken cancellationToken);

    Task<List<CommentResponse>> GetCommentsByPost(Guid postId, CancellationToken cancellationToken);
}