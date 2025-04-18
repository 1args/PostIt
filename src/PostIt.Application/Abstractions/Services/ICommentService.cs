using PostIt.Application.Contracts.Requests.Comment;
using PostIt.Application.Contracts.Responses;

namespace PostIt.Application.Abstractions.Services;

public interface ICommentService
{
    Task<Guid> CreateComment(CreateCommentRequest request, CancellationToken cancellationToken);

    Task DeleteComment(Guid commentId, CancellationToken cancellationToken);

    Task LikeCommentAsync(Guid commentId, Guid authorId, CancellationToken cancellationToken);

    Task UnlikeCommentAsync(Guid commentId, Guid authorId, CancellationToken cancellationToken);

    Task<List<CommentResponse>> GetCommentsByPost(Guid postId, CancellationToken cancellationToken);
}