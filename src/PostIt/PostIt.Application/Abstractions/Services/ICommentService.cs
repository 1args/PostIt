using PostIt.Contracts.ApiContracts.Requests.Comment;
using PostIt.Contracts.ApiContracts.Responses;
using PostIt.Contracts.Models.Pagination;
using PostIt.Contracts.Models.Sorting;

namespace PostIt.Application.Abstractions.Services;

/// <summary>
/// Provides functionality for managing comments.
/// </summary>
public interface ICommentService
{
    /// <summary>
    /// Creates a new comment for a post.
    /// </summary>
    /// <param name="request">Request containing the data needed to create the comment.</param>
    /// <param name="cancellationToken">Token to cancel the operation</param>
    /// <returns>The unique identifier of the newly created comment.</returns>
    Task<Guid> CreateCommentAsync(CreateCommentRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes an existing comment by its ID.
    /// </summary>
    /// <param name="commentId">ID of the comment.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task DeleteCommentAsync(Guid commentId, CancellationToken cancellationToken);

    /// <summary>
    /// Likes a comment by its ID.
    /// </summary>
    /// <param name="commentId">ID of the comment.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task LikeCommentAsync(Guid commentId, CancellationToken cancellationToken);

    /// <summary>
    /// Unlikes a comment by its ID.
    /// </summary>
    /// <param name="commentId">ID of the comment.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UnlikeCommentAsync(Guid commentId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a list of comments associated with a specific post.
    /// </summary>
    /// <param name="postId">ID of the post for which to fetch comments.</param>
    /// <param name="sortParams">ID of the post for which to fetch comments.</param>
    /// <param name="paginationParams">Pagination parameters.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>List of comments for the post.</returns>
    Task<Paginated<CommentResponse>> GetCommentsByPostAsync(Guid postId, SortParams? sortParams, PaginationParams paginationParams, CancellationToken cancellationToken);
}