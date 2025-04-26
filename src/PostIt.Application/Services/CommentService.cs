using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using PostIt.Application.Abstractions.Data;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Exceptions;
using PostIt.Contracts.ApiContracts.Requests.Comment;
using PostIt.Contracts.ApiContracts.Responses;
using PostIt.Contracts.Mappers;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects.Comment;

namespace PostIt.Application.Services;

public class CommentService(
    IRepository<Comment> commentRepository,
    IRepository<Post> postRepository,
    IRepository<User> userRepository,
    ILogger<CommentService> logger) : ICommentService
{
    public async Task<Guid> CreateCommentAsync(
        CreateCommentRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Creating comment from author with ID `{AuthorId}` to post with ID `{PostId}`.",
            request.AuthorId,
            request.PostId);
        
        var text = Text.Create(request.Text);

        var state = await postRepository.AnyAsync(p => p.Id == request.PostId, cancellationToken);

        if (!state)
        {
            logger.LogWarning("Post with ID '{PostId}' not found.", request.PostId);
            throw new NotFoundException($"Post with ID '{request.PostId}' not found.");
        }
        
        var comment = Comment.Create(text, request.AuthorId, request.PostId, DateTime.UtcNow);
        
        await commentRepository.AddAsync(comment, cancellationToken);

        var user = await GetUserOrThrowAsync(request.AuthorId,cancellationToken);
        user.IncrementCommentsCount();
        
        await userRepository.UpdateAsync(user, cancellationToken);
        
        logger.LogInformation(
            "Comment created successfully with ID `{CommentId}` to post with ID `{PostId}`.",
            comment.Id,
            comment.PostId);
        
        return comment.Id;
    }

    public async Task DeleteCommentAsync(
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
        
        var comment = await GetCommentOrThrowAsync(commentId, cancellationToken, includes: [p => p.Likes]);
        
        comment.Unlike(authorId);
        await commentRepository.UpdateAsync(comment, cancellationToken);
        
        logger.LogInformation(
            "Comment with ID `{CommentId}` unliked by user `{AuthorId}` successfully.",
            commentId,
            authorId);
    }

    public async Task<List<CommentResponse>> GetCommentsByPostAsync(
        Guid postId,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching comments for post with ID `{PostId}`.", postId);

        var comments = await commentRepository.ToListAsync(
            expression:c => c.PostId == postId,
            cancellationToken: cancellationToken,
            tracking: false);

        var sortedComment = comments
            .OrderByDescending(c => c.Likes.Count)
            .ThenByDescending(c => c.CreatedAt)
            .ToList();
        
        logger.LogInformation(
            "Fetched `{Count}` comments for post with ID `{PostId}`.",
            comments.Count, 
            postId);
        
        return sortedComment
            .Select(c => c.MapToPublic())
            .ToList();
    }

    private async Task<Comment> GetCommentOrThrowAsync(
        Guid commentId,
        CancellationToken cancellationToken,
        params Expression<Func<Comment, object>>[] includes)
    {
        var comment = await commentRepository
            .SingleOrDefaultAsync(c => c.Id == commentId, cancellationToken, includes: includes);
        
        if (comment is null)
        {
            logger.LogWarning("Comment with ID `{CommentId}` not found.", commentId);
            throw new NotFoundException($"Comment with ID '{commentId}' not found.");
        }
        
        logger.LogInformation("Comment with ID `{CommentId}` retrieved successfully.", commentId);
        
        return comment;
    }

    private async Task<User> GetUserOrThrowAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
        {
            logger.LogWarning("User with ID `{UserId}` not found.", userId);
            throw new NotFoundException($"User with ID '{userId}' not found.");
        }
        
        logger.LogInformation("User with ID `{UserId}` retrieved successfully.", userId);
        return user;
    }
}