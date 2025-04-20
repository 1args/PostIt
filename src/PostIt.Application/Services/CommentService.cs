using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Exceptions;
using PostIt.Contracts.ApiContracts.Requests.Comment;
using PostIt.Contracts.ApiContracts.Responses;
using PostIt.Contracts.Mappers;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects.Comment;
using PostIt.Infrastructure.Configuration.Repositories;

namespace PostIt.Application.Services;

public class CommentService(
    IRepository<Comment> commentRepository,
    IRepository<Post> postRepository,
    ILogger<CommentService> logger) : ICommentService
{
    public async Task<Guid> CreateComment(
        CreateCommentRequest request,
        CancellationToken cancellationToken)
    {
        var text = Text.Create(request.Text);

        var post = await postRepository
            .AsQueryable()
            .AsNoTracking()
            .Where(p => p.Id == request.PostId)
            .SingleOrDefaultAsync(cancellationToken);

        if (post is null)
        {
            throw new NotFoundException($"Post with ID '{request.PostId}' not found.");
        }

        var comment = Comment.Create(text, request.AuthorId, post.Id, DateTime.UtcNow);
        
        await commentRepository.AddAsync(comment, cancellationToken);
        return comment.Id;
    }

    public async Task DeleteComment(
        Guid commentId,
        CancellationToken cancellationToken)
    {
        var comment = await GetCommentOrThrowAsync(commentId, cancellationToken);

        await commentRepository.DeleteAsync([comment], cancellationToken);
    }

    public async Task LikeCommentAsync(
        Guid commentId,
        Guid authorId,
        CancellationToken cancellationToken)
    {
        var comment = await GetCommentOrThrowAsync(commentId, cancellationToken);
        
        comment.Like(authorId);
        await commentRepository.UpdateAsync(comment, cancellationToken);
    }
    
    public async Task UnlikeCommentAsync(
        Guid commentId,
        Guid authorId,
        CancellationToken cancellationToken)
    {
        var comment = await GetCommentOrThrowAsync(commentId, cancellationToken);
        
        comment.Unlike(authorId);
        await commentRepository.UpdateAsync(comment, cancellationToken);
    }

    public async Task<List<CommentResponse>> GetCommentsByPost(
        Guid postId,
        CancellationToken cancellationToken)
    {
        var posts = await commentRepository
            .AsQueryable()
            .AsNoTracking()
            .Where(c => c.PostId == postId)
            .OrderByDescending(c => c.Likes.Count)
            .ThenByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
        
        return posts
            .Select(c => c.MapToPublic())
            .ToList();
    }

    private async Task<Comment> GetCommentOrThrowAsync(
        Guid commentId,
        CancellationToken cancellationToken)
    {
        var query = commentRepository.AsQueryable();
        
        var comment = await commentRepository
            .Where(c => c.Id == commentId)
            .SingleOrDefaultAsync(cancellationToken);
        
        return comment ?? throw new NotFoundException($"Comment with ID '{commentId}' not found.");
    }
}