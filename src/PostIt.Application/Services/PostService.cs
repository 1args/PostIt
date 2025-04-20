using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Exceptions;
using PostIt.Contracts.ApiContracts.Requests.Post;
using PostIt.Contracts.ApiContracts.Responses;
using PostIt.Contracts.Mappers;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects.Post;
using PostIt.Infrastructure.Configuration.Repositories;

namespace PostIt.Application.Services;

public class PostService(
    IRepository<Post> postRepository,
    ILogger<PostService> logger) : IPostService
{
    public async Task<Guid> CreatePostAsync(
        CreatePostRequest request,
        CancellationToken cancellationToken)
    {
        var title = Title.Create(request.Title);
        var content = Content.Create(request.Content);

        var post = Post.Create(
            title, 
            content,
            request.AuthorId,
            DateTime.UtcNow,
            request.Visibility);

        await postRepository.AddAsync(post, cancellationToken);
        
        return post.Id;
    }
    public async Task UpdatePostAsync(
        UpdatePostRequest request,
        CancellationToken cancellationToken)
    {
        var post = await GetPostOrThrowAsync(request.PostId, cancellationToken);
        
        var newTitle = Title.Create(request.Title);
        var newContent = Content.Create(request.Content);
        
        post.UpdateContent(newTitle, newContent);
        await postRepository.UpdateAsync(post, cancellationToken);
    }

    public async Task DeletePostAsync(
        Guid postId, 
        CancellationToken cancellationToken)
    {
        var post = await GetPostOrThrowAsync(postId, cancellationToken);

        await postRepository.DeleteAsync([post], cancellationToken);
    }
    
    public async Task LikePostAsync(
        Guid postId,
        Guid authorId,
        CancellationToken cancellationToken)
    {
        var post = await GetPostOrThrowAsync(postId, cancellationToken); 

        post.Like(authorId);
        await postRepository.UpdateAsync(post, cancellationToken);
    }

    public async Task UnlikePostAsync(
        Guid postId,
        Guid authorId,
        CancellationToken cancellationToken)
    {
        var post = await GetPostOrThrowAsync(postId, cancellationToken);
        
        post.Unlike(authorId);
        await postRepository.UpdateAsync(post, cancellationToken);
    }

    public async Task ViewPostAsync(
        Guid postId,
        CancellationToken cancellationToken)
    {
        var post = await GetPostOrThrowAsync(postId, cancellationToken);
        
        post.View();
        await postRepository.UpdateAsync(post, cancellationToken);
    }

    public async Task ChangeVisibilityAsync(
        ChangePostVisibilityRequest request,
        CancellationToken cancellationToken)
    {
        var post = await GetPostOrThrowAsync(request.PostId, cancellationToken);
        
        post.SetVisibility(request.Visibility);
        await postRepository.UpdateAsync(post, cancellationToken);
    }

    public async Task<List<PostResponse>> GetPostsSortedByLikesAsync(
        Guid currentUserId,
        CancellationToken cancellationToken)
    {
        var posts = await postRepository
            .AsQueryable()
            .AsNoTracking()
            .Include(p => p.Likes)
            .Include(p => p.Comments)
            .OrderByDescending(p => p.Likes.Count)
            .ToListAsync(cancellationToken);

        return posts
            .Select(p => p.MapToPublic(currentUserId))
            .ToList();
    }

    public async Task<List<PostResponse>> GetPostsSortedByViewsAsync(
        Guid currentUserId,
        CancellationToken cancellationToken)
    {
        var posts = await postRepository
            .AsQueryable()
            .AsNoTracking()
            .Include(p => p.Likes)
            .Include(p => p.Comments)
            .OrderByDescending(p => p.Views)
            .ToListAsync(cancellationToken);

        return posts
            .Select(p => p.MapToPublic(currentUserId))
            .ToList();
    }
    
    private async Task<Post> GetPostOrThrowAsync(
        Guid postId, 
        CancellationToken cancellationToken)
    {
        var post = await postRepository
            .Where(p => p.Id == postId)
            .SingleOrDefaultAsync(cancellationToken);
        
        return post ?? throw new NotFoundException($"Post with ID '{postId}' not found.");
    }
}