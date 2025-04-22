using System.Linq.Expressions;
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
    ILogger<PostService> logger) : IPostService
{
    public async Task<Guid> CreatePostAsync(
        CreatePostRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating post by author ID `{AuthorId}`.", request.AuthorId);
        
        var title = Title.Create(request.Title);
        var content = Content.Create(request.Content);

        var post = Post.Create(
            title, 
            content,
            request.AuthorId,
            DateTime.UtcNow,
            request.Visibility);

        await postRepository.AddAsync(post, cancellationToken);
        
        var user = await userRepository.SingleOrDefaultAsync(u => u.Id == post.AuthorId, cancellationToken);
        
        if (user is not null)
        {
            user.IncrementPostsCount();
            await userRepository.UpdateAsync(user, cancellationToken);
        }

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

        await postRepository.DeleteAsync([post], cancellationToken);
        
        var user = await userRepository.SingleOrDefaultAsync(u => u.Id == post.AuthorId, cancellationToken);

        if (user is not null)
        {
            user.DecrementPostsCount();
            await userRepository.UpdateAsync(user, cancellationToken);
        }
        
        logger.LogInformation("Post with ID `{PostId}` deleted successfully.", postId);
    }
    
    public async Task LikePostAsync(
        Guid postId,
        Guid authorId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Liking post `{PostId}` by user `{AuthorId}`.", postId, authorId);
        
        var post = await GetPostOrThrowAsync(postId, cancellationToken); 

        post.Like(authorId);
        await postRepository.UpdateAsync(post, cancellationToken);
        
        logger.LogInformation("Post `{PostId}` liked by user `{AuthorId}`.", postId, authorId);
    }

    public async Task UnlikePostAsync(
        Guid postId,
        Guid authorId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Unliking post `{PostId}` by user `{AuthorId}`.", postId, authorId);
        
        var post = await GetPostOrThrowAsync(postId, cancellationToken, includes: [p => p.Likes]);
        
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
        
        post.SetVisibility(request.Visibility);
        await postRepository.UpdateAsync(post, cancellationToken);
        
        logger.LogInformation(
            "Visibility for post `{PostId}` changed to `{Visibility}`.",
            postId, 
            request.Visibility);
    }

    public async Task<List<PostResponse>> GetPostsSortedByLikesAsync(
        Guid currentUserId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching posts sorted by likes.");

        var posts = await postRepository
            .ToListAsync(cancellationToken: cancellationToken, tracking: false);
        
        var sortedPosts = posts
            .OrderByDescending(p => p.Likes.Count)
            .ToList();

        logger.LogInformation("Fetched `{Count}` posts sorted by likes.", posts.Count);
        
        return sortedPosts
            .Select(p => p.MapToPublic(currentUserId))
            .ToList();
    }

    public async Task<List<PostResponse>> GetPostsSortedByViewsAsync(
        Guid currentUserId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching posts sorted by views.");
        
        var posts = await postRepository
            .ToListAsync(cancellationToken: cancellationToken, tracking: false);
        
        var sortedPosts = posts
            .OrderByDescending(p => p.ViewCount)
            .ToList();
        
        logger.LogInformation("Fetched `{Count}` posts sorted by views.", posts.Count);
        
        return sortedPosts
            .Select(p => p.MapToPublic(currentUserId))
            .ToList();
    }
    
    private async Task<Post> GetPostOrThrowAsync(
        Guid postId, 
        CancellationToken cancellationToken,
        params Expression<Func<Post, object>>[] includes)
    {
        var post =await postRepository
            .SingleOrDefaultAsync(p => p.Id == postId, cancellationToken, tracking: true, includes);
        
        if (post is null)
        {
            logger.LogWarning("Post with ID `{PostId}` not found.", postId);
            throw new NotFoundException($"Post with ID '{postId}' not found.");
        }
        
        logger.LogInformation("Post with ID `{PostId}` retrieved successfully.", postId);

        return post;
    }
}