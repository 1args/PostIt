using PostIt.Contracts.ApiContracts.Requests.Post;
using PostIt.Contracts.ApiContracts.Responses;

namespace PostIt.Application.Abstractions.Services;

/// <summary>
/// Provides functionality for managing posts.
/// </summary>
public interface IPostService
{
    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="request">Request containing the data required to create the post.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>ID of the newly created post.</returns>
    Task<Guid> CreatePostAsync(CreatePostRequest request, CancellationToken cancellationToken);
    
    /// <summary>
    /// Updates an existing post by its ID.
    /// </summary>
    /// <param name="postId">ID of the post to update.</param>
    /// <param name="request">Request containing the updated data for the post.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UpdatePostAsync(Guid postId, UpdatePostRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a post by its ID.
    /// </summary>
    /// <param name="postId">ID of the post to delete.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task DeletePostAsync(Guid postId, CancellationToken cancellationToken);

    /// <summary>
    /// Likes a post by its ID.
    /// </summary>
    /// <param name="postId">ID of the post to like.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task LikePostAsync(Guid postId, CancellationToken cancellationToken);

    /// <summary>
    /// Unlikes a liked post by its ID.
    /// </summary>
    /// <param name="postId">ID of the post to unlike.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UnlikePostAsync(Guid postId, CancellationToken cancellationToken);

    /// <summary>
    /// Marks a post as viewed by its ID.
    /// </summary>
    /// <param name="postId">ID of the post to mark as viewed.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task ViewPostAsync(Guid postId, CancellationToken cancellationToken);

    /// <summary>
    /// Changes the visibility of a post by its ID.
    /// </summary>
    /// <param name="postId">ID of the post whose visibility will be changed.</param>
    /// <param name="request">Request containing the new visibility settings for the post.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task ChangeVisibilityAsync(Guid postId, ChangePostVisibilityRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a list of all posts.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>List of all posts.</returns>
    Task<List<PostResponse>> GetAllPosts(CancellationToken cancellationToken);
}