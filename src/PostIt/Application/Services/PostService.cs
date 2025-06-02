using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostIt.Application.Abstractions.Data;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Abstractions.Utilities;
using PostIt.Common.Extensions;
using PostIt.Contracts.Exceptions;
using PostIt.Contracts.Mappers;
using PostIt.Contracts.Models.Pagination;
using PostIt.Contracts.Models.Sorting;
using PostIt.Contracts.Requests.Post;
using PostIt.Contracts.Responses;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects;
using Permission = PostIt.Domain.Enums.Permission;

namespace PostIt.Application.Services;

/// <inheritdoc/>
public class PostService(
    IRepository<Post> postRepository,
    IAuthenticationService authenticationService,
    IPermissionChecker<Post> permissionChecker,
    ILogger<PostService> logger) : IPostService
{
    /// <inheritdoc/>
    public async Task<Guid> CreatePostAsync(
        CreatePostRequest request,
        CancellationToken cancellationToken)
    {
        var authorId = GetCurrentUserId();
        
        logger.LogInformation("Creating post by author ID `{AuthorId}`.", authorId);
        
        var title = PostTitle.Create(request.Title);
        var content = PostContent.Create(request.Content);

        var post = Post.Create(
            title, 
            content,
            authorId,
            DateTime.UtcNow,
            request.Visibility);

        await postRepository.AddAsync(post, cancellationToken);

        logger.LogInformation("Post with ID `{PostId}` created successfully.", post.Id);
        
        return post.Id;
    }
    
    /// <inheritdoc/>
    public async Task UpdatePostAsync(
        Guid postId,
        UpdatePostRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating post with ID `{PostId}`.", postId);
        
        var post = await GetPostAsync(postId, cancellationToken);
        
        var authorId = GetCurrentUserId();
        
        await permissionChecker.CheckPermissionsAsync(
            post,
            authorId, 
            Permission.EditOwnPost,
            Permission.EditAnyPost,
            cancellationToken);
        
        var newTitle = PostTitle.Create(request.Title);
        var newContent = PostContent.Create(request.Content);
        
        post.UpdateContent(newTitle, newContent);
        await postRepository.UpdateAsync(post, cancellationToken);
        
        logger.LogInformation(
            "Post with ID `{PostId}` updated successfully by user `{authorId}`.",
            postId,
            authorId);
    }

    /// <inheritdoc/>
    public async Task DeletePostAsync(
        Guid postId, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting post with ID `{PostId}`.", postId);
        
        var post = await GetPostAsync(postId, cancellationToken);
     
        var authorId = GetCurrentUserId();
        
        await permissionChecker.CheckPermissionsAsync(
            post,
            authorId, 
            Permission.DeleteOwnPost,
            Permission.DeleteAnyPost,
            cancellationToken);
        
        await postRepository.DeleteAsync([post], cancellationToken);
        
        logger.LogInformation(
            "Post with ID `{PostId}` deleted successfully by user `{authorId}`.",
            postId,
            authorId);
    }
    
    /// <inheritdoc/>
    public async Task LikePostAsync(
        Guid postId,
        CancellationToken cancellationToken)
    {
        var authorId = GetCurrentUserId();
        
        logger.LogInformation("Liking post `{PostId}` by user `{AuthorId}`.", postId, authorId);
        
        var post = await postRepository
            .AsQueryable()
            .Include(p => p.Likes)
            .SingleOrDefaultAsync(p => p.Id == postId, cancellationToken);

        if (post is null)
        {
            logger.LogInformation("Post with ID `{PostId}` was not found.", postId);
            throw new NotFoundException("Post with ID '{PostId}' was not found.");
        }

        post.Like(authorId);
        await postRepository.UpdateAsync(post, cancellationToken);
        
        logger.LogInformation("Post `{PostId}` liked by user `{AuthorId}`.", postId, authorId);
    }

    /// <inheritdoc/>
    public async Task UnlikePostAsync(
        Guid postId,
        CancellationToken cancellationToken)
    {
        var authorId = GetCurrentUserId();
        
        logger.LogInformation("Unliking post `{PostId}` by user `{AuthorId}`.", postId, authorId);

        var post = await postRepository
            .AsQueryable()
            .Include(p => p.Likes)
            .SingleOrDefaultAsync(p => p.Id == postId, cancellationToken);

        if (post is null)
        {
            logger.LogInformation("Post with ID `{PostId}` was not found.", postId);
            throw new NotFoundException("Post with ID '{PostId}' was not found.");
        }

        post.Unlike(authorId);
        await postRepository.UpdateAsync(post, cancellationToken);
        
        logger.LogInformation("Post `{PostId}` unliked by `{AuthorId}`.", postId, authorId);
    }

    /// <inheritdoc/>
    public async Task ViewPostAsync(
        Guid postId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Viewing post with ID `{PostId}`.", postId);
        
        var post = await GetPostAsync(postId, cancellationToken);
        
        post.View();
        await postRepository.UpdateAsync(post, cancellationToken);
        
        logger.LogInformation("Post with ID `{PostId}` viewed.", postId);
    }

    /// <inheritdoc/>
    public async Task ChangeVisibilityAsync(
        Guid postId,
        ChangePostVisibilityRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Changing visibility for post `{PostId}` to `{Visibility}`.", 
            postId,
            request.Visibility);

        var post = await GetPostAsync(postId, cancellationToken);
        
        var authorId = GetCurrentUserId();
        
        await permissionChecker.CheckPermissionsAsync(
            post,
            authorId, 
            Permission.EditOwnPost,
            Permission.EditAnyPost,
            cancellationToken);
        
        post.SetVisibility(request.Visibility);
        await postRepository.UpdateAsync(post, cancellationToken);
        
        logger.LogInformation(
            "Visibility for post `{PostId}` changed to `{Visibility}` by user `{authorId}`.",
            postId, 
            request.Visibility,
            authorId);
    }

    /// <inheritdoc/>
    public async Task<Paginated<PostResponse>> PagingPostsAsync(
        SortParams? sortParams,
        PaginationParams paginationParams, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching posts sorted by likes.");

        var paginatedPosts = await postRepository
            .AsQueryable()
            .AsNoTracking()
            .Include(p => p.Likes)
            .SortedBy(sortParams)
            .AsPaginatedAsync(paginationParams, cancellationToken);
        
        var postsResponses = paginatedPosts.Items
            .Select(p => p.MapToPublic())
            .ToList()
            .AsReadOnly();
        
        logger.LogInformation("Fetched `{Count}` posts sorted by likes.", postsResponses.Count);
        
        return new Paginated<PostResponse>
        {
            Items = postsResponses,
            PaginationParams = paginationParams,
            TotalPages = paginatedPosts.TotalPages,
            HasPreviousPage = paginatedPosts.HasPreviousPage,
            HasNextPage = paginatedPosts.HasNextPage
        };
    }
    
    private Guid GetCurrentUserId() => authenticationService.GetUserIdFromAccessToken();
    
    private async Task<Post> GetPostAsync(
        Guid postId, 
        CancellationToken cancellationToken)
    {
        var post =await postRepository
            .AsQueryable()
            .SingleOrDefaultAsync(p => p.Id == postId, cancellationToken);
        
        if (post is null)
        {
            logger.LogWarning("Post with ID `{PostId}` not found.", postId);
            throw new NotFoundException($"Post with ID '{postId}' not found.");
        }
        
        logger.LogInformation("Post with ID `{PostId}` retrieved successfully.", postId);

        return post;
    }
}