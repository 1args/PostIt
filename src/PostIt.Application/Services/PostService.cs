using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostIt.Application.Abstractions.Data;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Exceptions;
using PostIt.Contracts.ApiContracts.Requests.Post;
using PostIt.Contracts.ApiContracts.Responses;
using PostIt.Contracts.Mappers;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects.Post;

namespace PostIt.Application.Services;

public class PostService(
    IRepository<Post> postRepository,
    IRepository<User> userRepository,
    IAuthenticationService authenticationService,
    ILogger<PostService> logger) : IPostService
{
    public async Task<Guid> CreatePostAsync(
        CreatePostRequest request,
        CancellationToken cancellationToken)
    {
        var authorId = GetCurrentUserId();
        
        logger.LogInformation("Creating post by author ID `{AuthorId}`.", authorId);
        
        var title = Title.Create(request.Title);
        var content = Content.Create(request.Content);

        var post = Post.Create(
            title, 
            content,
            authorId,
            DateTime.UtcNow,
            request.Visibility);

        await postRepository.AddAsync(post, cancellationToken);

        await UpdateUserPostCountAsync(authorId, increase: true, cancellationToken);

        logger.LogInformation("Post with ID `{PostId}` created successfully.", post.Id);
        
        return post.Id;
    }
    public async Task UpdatePostAsync(
        Guid postId,
        UpdatePostRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating post with ID `{PostId}`.", postId);
        
        var post = await GetPostOrThrowAsync(postId, cancellationToken);
        
        var authorId = GetCurrentUserId();
        EnsureUserIsAuthorOrThrow(post, authorId);
        
        var newTitle = Title.Create(request.Title);
        var newContent = Content.Create(request.Content);
        
        post.UpdateContent(newTitle, newContent);
        await postRepository.UpdateAsync(post, cancellationToken);
        
        logger.LogInformation("Post with ID `{PostId}` updated successfully.", postId);
    }

    public async Task DeletePostAsync(
        Guid postId, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting post with ID `{PostId}`.", postId);
        
        var post = await GetPostOrThrowAsync(postId, cancellationToken);
     
        var authorId = GetCurrentUserId();
        EnsureUserIsAuthorOrThrow(post, authorId);
        
        await postRepository.DeleteAsync([post], cancellationToken);
        
        await UpdateUserPostCountAsync(authorId, increase: false, cancellationToken);
        
        logger.LogInformation("Post with ID `{PostId}` deleted successfully.", postId);
    }
    
    public async Task LikePostAsync(
        Guid postId,
        CancellationToken cancellationToken)
    {
        var authorId = GetCurrentUserId();
        
        logger.LogInformation("Liking post `{PostId}` by user `{AuthorId}`.", postId, authorId);
        
        var post = await postRepository
            .AsQueryable()
            .Include(p => p.Likes)
            .SingleOrDefaultAsync(cancellationToken);

        if (post is null)
        {
            logger.LogInformation("Post with ID `{PostId}` was not found.", postId);
            throw new NotFoundException("Post with ID '{PostId}' was not found.");
        }

        post.Like(authorId);
        await postRepository.UpdateAsync(post, cancellationToken);
        
        logger.LogInformation("Post `{PostId}` liked by user `{AuthorId}`.", postId, authorId);
    }

    public async Task UnlikePostAsync(
        Guid postId,
        CancellationToken cancellationToken)
    {
        var authorId = GetCurrentUserId();
        
        logger.LogInformation("Unliking post `{PostId}` by user `{AuthorId}`.", postId, authorId);

        var post = await postRepository
            .AsQueryable()
            .Include(p => p.Likes)
            .SingleOrDefaultAsync(cancellationToken);

        if (post is null)
        {
            logger.LogInformation("Post with ID `{PostId}` was not found.", postId);
            throw new NotFoundException("Post with ID '{PostId}' was not found.");
        }

        post.Unlike(authorId);
        await postRepository.UpdateAsync(post, cancellationToken);
        
        logger.LogInformation("Post `{PostId}` unliked by `{AuthorId}`.", postId, authorId);
    }

    public async Task ViewPostAsync(
        Guid postId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Viewing post with ID `{PostId}`.", postId);
        
        var post = await GetPostOrThrowAsync(postId, cancellationToken);
        
        post.View();
        await postRepository.UpdateAsync(post, cancellationToken);
        
        logger.LogInformation("Post with ID `{PostId}` viewed.", postId);
    }

    public async Task ChangeVisibilityAsync(
        Guid postId,
        ChangePostVisibilityRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Changing visibility for post `{PostId}` to `{Visibility}`.", 
            postId,
            request.Visibility);

        var post = await GetPostOrThrowAsync(postId, cancellationToken);
        
        var authorId = GetCurrentUserId();
        EnsureUserIsAuthorOrThrow(post, authorId);
        
        post.SetVisibility(request.Visibility);
        await postRepository.UpdateAsync(post, cancellationToken);
        
        logger.LogInformation(
            "Visibility for post `{PostId}` changed to `{Visibility}`.",
            postId, 
            request.Visibility);
    }

    public async Task<List<PostResponse>> GetPostsSortedByLikesAsync(
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching posts sorted by likes.");
        
        var currentUserId = GetCurrentUserId();

        var posts = await postRepository
            .AsQueryable()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        var sortedPosts = posts
            .OrderByDescending(p => p.Likes.Count)
            .ToList();

        logger.LogInformation("Fetched `{Count}` posts sorted by likes.", posts.Count);
        
        return sortedPosts
            .Select(p => p.MapToPublic(currentUserId))
            .ToList();
    }

    public async Task<List<PostResponse>> GetPostsSortedByViewsAsync(
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching posts sorted by views.");
        
        var currentUserId = GetCurrentUserId();
        
        var posts = await postRepository
            .AsQueryable()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        var sortedPosts = posts
            .OrderByDescending(p => p.ViewCount)
            .ToList();
        
        logger.LogInformation("Fetched `{Count}` posts sorted by views.", posts.Count);
        
        return sortedPosts
            .Select(p => p.MapToPublic(currentUserId))
            .ToList();
    }
    
    private async Task UpdateUserPostCountAsync(Guid userId, bool increase, CancellationToken cancellationToken)
    {
        var user = await userRepository
            .AsQueryable()
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);
        
        if (user is null)
        {
            logger.LogWarning("User with ID `{UserId}` no found while updating post count.", userId);
            return;
        }
        if (increase)
        {
            user.IncrementPostsCount();
        }
        else
        {
            user.DecrementPostsCount();
        }

        await userRepository.UpdateAsync(user, cancellationToken);
    }
    
    private Guid GetCurrentUserId() => authenticationService.GetUserIdFromAccessToken();

    private void EnsureUserIsAuthorOrThrow(Post post, Guid currentUserId)
    {
        if (post.AuthorId != currentUserId)
        {
            logger.LogWarning(
                "User `{UserId}` attempted to perform an unauthorized action on post `{PostId}`.",
                currentUserId,
                post.Id);
            
            throw new UnauthorizedException("Only the author is allowed to perform this action on the post.");
        }
    }
    
    private async Task<Post> GetPostOrThrowAsync(
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