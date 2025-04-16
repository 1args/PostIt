using PostIt.Application.Contracts.Requests.Comment;
using PostIt.Application.Contracts.Responses;

namespace PostIt.Application.Abstractions.Services.Services;

public interface ICommentService
{
    Task<Guid> CreateComment(CreateCommentRequest request, CancellationToken cancellationToken = default);

    Task DeleteComment(Guid commentId, CancellationToken cancellationToken = default);

    Task LikeCommentAsync(Guid commentId, Guid authorId, CancellationToken cancellationToken = default);

    Task UnlikeCommentAsync(Guid commentId, Guid authorId, CancellationToken cancellationToken = default);

    Task<List<CommentResponse>> GetCommentsByPost(Guid postId, CancellationToken cancellationToken = default);
}