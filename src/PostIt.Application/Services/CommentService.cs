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
        logger.LogInformation(
            "Creating comment from author with ID `{AuthorId}` to post with ID `{PostId}`.",
            request.AuthorId,
            request.PostId);
        
        var text = Text.Create(request.Text);

        var post = await postRepository
            .AsQueryable()
            .AsNoTracking()
            .Where(p => p.Id == request.PostId)
            .SingleOrDefaultAsync(cancellationToken);

        if (post is null)
        {
            logger.LogWarning("Post with ID `{PostId}` not found. Cannot create comment.", request.PostId);
            throw new NotFoundException($"Post with ID `{request.PostId}` not found.");
        }

        var comment = Comment.Create(text, request.AuthorId, post.Id, DateTime.UtcNow);
        
        await commentRepository.AddAsync(comment, cancellationToken);
        
        logger.LogInformation(
            "Comment created successfully with ID `{CommentId}` to post with ID `{PostId}`.",
            comment.Id,
            comment.PostId);
        
        return comment.Id;
    }

    public async Task DeleteComment(
        Guid commentId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting comment with ID `{CommentId}`.", commentId);
        
        var comment = await GetCommentOrThrowAsync(commentId, cancellationToken);

        await commentRepository.DeleteAsync([comment], cancellationToken);
        logger.LogInformation("Comment with ID `{CommentId}` deleted successfully.", commentId);
    }

    public async Task LikeCommentAsync(
        Guid commentId,
        Guid authorId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Liking comment with ID `{CommentId}` by user `{AuthorId}`.",
            commentId,
            authorId);
        
        var comment = await GetCommentOrThrowAsync(commentId, cancellationToken);
        
        comment.Like(authorId);
        await commentRepository.UpdateAsync(comment, cancellationToken);
        
        logger.LogInformation(
            "Comment with ID `{CommentId}` liked by user `{AuthorId}` successfully.", 
            commentId,
            authorId);
    }
    
    public async Task UnlikeCommentAsync(
        Guid commentId,
        Guid authorId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Unliking comment with ID `{CommentId}` by user `{AuthorId}`.", 
            commentId,
            authorId);
        
        var comment = await GetCommentOrThrowAsync(commentId, cancellationToken);
        
        comment.Unlike(authorId);
        await commentRepository.UpdateAsync(comment, cancellationToken);
        
        logger.LogInformation(
            "Comment with ID `{CommentId}` unliked by user `{AuthorId}` successfully.",
            commentId,
            authorId);
    }

    public async Task<List<CommentResponse>> GetCommentsByPost(
        Guid postId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching comments for post with ID `{PostId}`.", postId);
        
        var comments = await commentRepository
            .AsQueryable()
            .AsNoTracking()
            .Where(c => c.PostId == postId)
            .OrderByDescending(c => c.Likes.Count)
            .ThenByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
        
        logger.LogInformation(
            "Fetched `{Count}` comments for post with ID `{PostId}`.",
            comments.Count, 
            postId);
        
        return comments
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
        
        if (comment is null)
        {
            logger.LogWarning("Comment with ID `{CommentId}` not found.", commentId);
            throw new NotFoundException($"Comment with ID '{commentId}' not found.");
        }
        
        logger.LogInformation("Comment with ID `{CommentId}` retrieved successfully.", commentId);
        
        return comment;
    }
}