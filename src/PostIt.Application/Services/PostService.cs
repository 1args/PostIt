using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostIt.Application.Contracts.Requests.Post;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects.Post;
using PostIt.Infrastructure.Configuration.Repositories;

namespace PostIt.Application.Services;

public class PostService(
    IRepository<Post> postRepository,
    ILogger<PostService> logger)
{
    public async Task<Guid> CreatePostAsync(
        CreatePostRequest request,
        CancellationToken cancellationToken = default)
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
        CancellationToken cancellationToken = default)
    {
        var post = await GetPostOrThrowAsync(request.PostId, cancellationToken);
        
        var newTitle = Title.Create(request.Title);
        var newContent = Content.Create(request.Content);
        
        post.UpdateContent(newTitle, newContent);
        await postRepository.UpdateAsync(post, cancellationToken);
    }

    public async Task DeletePostAsync(
        Guid postId, 
        CancellationToken cancellationToken = default)
    {
        var post = await GetPostOrThrowAsync(postId, cancellationToken);

        await postRepository.DeleteAsync([post], cancellationToken);
    }
    
    public async Task LikePostAsync(
        Guid postId,
        Guid authorId,
        CancellationToken cancellationToken = default)
    {
        var post = await GetPostOrThrowAsync(postId, cancellationToken); 

        post.Like(authorId);
        await postRepository.UpdateAsync(post, cancellationToken);
    }

    public async Task UnlikePostAsync(
        Guid postId,
        Guid authorId,
        CancellationToken cancellationToken = default)
    {
        var post = await GetPostOrThrowAsync(postId, cancellationToken);
        
        post.Unlike(authorId);
        await postRepository.UpdateAsync(post, cancellationToken);
    }

    public async Task ViewPostAsync(
        Guid postId,
        CancellationToken cancellationToken = default)
    {
        var post = await GetPostOrThrowAsync(postId, cancellationToken);
        
        post.View();
        await postRepository.UpdateAsync(post, cancellationToken);
    }

    public async Task ChangeVisibilityAsync(
        ChangePostVisibilityRequest request,
        CancellationToken cancellationToken = default)
    {
        var post = await GetPostOrThrowAsync(request.PostId, cancellationToken);
        
        post.SetVisibility(request.Visibility);
        await postRepository.UpdateAsync(post, cancellationToken);
    }

    private async Task<Post> GetPostOrThrowAsync(
        Guid postId, 
        CancellationToken cancellationToken = default)
    {
        var post = await postRepository
            .Where(p => p.Id == postId)
            .FirstOrDefaultAsync(cancellationToken);
        
        return post ?? throw new InvalidOperationException($"Post with ID '{postId}' not found.");
    }
}