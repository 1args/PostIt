using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostIt.Application.Abstractions.Data;
using PostIt.Application.Abstractions.Services;
using PostIt.Application.Abstractions.Utilities;
using PostIt.Common.Extensions;
using PostIt.Contracts.ApiContracts.Requests.Comment;
using PostIt.Contracts.Exceptions;
using PostIt.Contracts.Mappers;
using PostIt.Contracts.Models.Pagination;
using PostIt.Contracts.Models.Sorting;
using PostIt.Contracts.Responses;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects;
using Permission = PostIt.Domain.Enums.Permission;

namespace PostIt.Application.Services;

/// <inheritdoc/>
public class CommentService(
    IRepository<Comment> commentRepository,
    IRepository<Post> postRepository,
    IAuthenticationService authenticationService,
    IPermissionChecker<Comment> permissionChecker,
    ILogger<CommentService> logger) : ICommentService
{
    /// <inheritdoc/>
    public async Task<Guid> CreateCommentAsync(
        CreateCommentRequest request,
        CancellationToken cancellationToken)
    {
        var authorId = GetCurrentUserId();
        
        logger.LogInformation(
            "Creating comment from author with ID `{AuthorId}` to post with ID `{PostId}`.",
            authorId,
            request.PostId);

        var text = CommentText.Create(request.Text);

        var postExists = await postRepository
            .AsQueryable()
            .AnyAsync(post => post.Id == request.PostId, cancellationToken);

        if (!postExists)
        {
            logger.LogWarning("Post with ID '{PostId}' not found.", request.PostId);
            throw new NotFoundException($"Post with ID '{request.PostId}' not found.");
        }
        
        var comment = Comment.Create(text, authorId, request.PostId, DateTime.UtcNow);
        
        await commentRepository.AddAsync(comment, cancellationToken);
        
        logger.LogInformation(
            "Comment created successfully with ID `{CommentId}` to post with ID `{PostId}`.",
            comment.Id,
            comment.PostId);
        
        return comment.Id;
    }

    /// <inheritdoc/>
    public async Task DeleteCommentAsync(
        Guid commentId,
        CancellationToken cancellationToken)
    {
        var authorId = GetCurrentUserId();
        
        logger.LogInformation("Deleting comment with ID `{CommentId}`.", commentId);
        
        var comment = await GetCommentAsync(commentId, cancellationToken);
        
        await permissionChecker.CheckPermissionsAsync(
            comment,
            authorId, 
            Permission.DeleteOwnComment,
            Permission.DeleteAnyComment,
            cancellationToken);

        await commentRepository.DeleteAsync([comment], cancellationToken);
        logger.LogInformation("Comment with ID `{CommentId}` deleted successfully.", commentId);
    }

    /// <inheritdoc/>
    public async Task LikeCommentAsync(
        Guid commentId,
        CancellationToken cancellationToken)
    {
        var authorId = GetCurrentUserId();
        
        logger.LogInformation(
            "Liking comment with ID `{CommentId}` by user `{AuthorId}`.",
            commentId,
            authorId);
        
        var comment = await commentRepository
            .AsQueryable()
            .Include(p => p.Likes)
            .SingleOrDefaultAsync(p => p.Id == commentId, cancellationToken);

        if (comment is null)
        {
            logger.LogWarning("Comment with ID `{CommentId}` not found.", commentId);
            throw new NotFoundException($"Comment with ID '{commentId}' not found.");
        }
        
        comment.Like(authorId);
        await commentRepository.UpdateAsync(comment, cancellationToken);
        
        logger.LogInformation(
            "Comment with ID `{CommentId}` liked by user `{AuthorId}` successfully.", 
            commentId,
            authorId);
    }
    
    /// <inheritdoc/>
    public async Task UnlikeCommentAsync(
        Guid commentId,
        CancellationToken cancellationToken)
    {
        var authorId = GetCurrentUserId();
        
        logger.LogInformation(
            "Unliking comment with ID `{CommentId}` by user `{AuthorId}`.", 
            commentId,
            authorId);

        var comment = await commentRepository
            .AsQueryable()
            .Include(p => p.Likes)
            .SingleOrDefaultAsync(p => p.Id == commentId, cancellationToken);

        if (comment is null)
        {
            logger.LogWarning("Comment with ID `{CommentId}` not found.", commentId);
            throw new NotFoundException($"Comment with ID '{commentId}' not found.");
        }
        
        comment.Unlike(authorId);
        await commentRepository.UpdateAsync(comment, cancellationToken);
        
        logger.LogInformation(
            "Comment with ID `{CommentId}` unliked by user `{AuthorId}` successfully.",
            commentId,
            authorId);
    }

    /// <inheritdoc/>
    public async Task<Paginated<CommentResponse>> PagingCommentsByPostAsync(
        Guid postId,
        SortParams? sortParams,
        PaginationParams paginationParams,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching comments for post with ID `{PostId}`.", postId);

        var paginatedComments = await commentRepository
            .AsQueryable()
            .AsNoTracking()
            .Include(c => c.Likes)
            .Where(c => c.PostId == postId)
            .SortedBy(sortParams)
            .AsPaginatedAsync(paginationParams, cancellationToken);

        var commentResponses = paginatedComments.Items
            .Select(c => c.MapToPublic())
            .ToList()
            .AsReadOnly();
        
        logger.LogInformation(
            "Fetched `{Count}` comments for post with ID `{PostId}`.",
            commentResponses.Count, 
            postId);
        
        return new Paginated<CommentResponse>
        {
            Items = commentResponses,
            PaginationParams = paginationParams,
            TotalPages = paginatedComments.TotalPages,
            HasPreviousPage = paginatedComments.HasPreviousPage,
            HasNextPage = paginatedComments.HasNextPage
        };
    }
    
    private async Task<Comment> GetCommentAsync(
        Guid commentId,
        CancellationToken cancellationToken)
    {
        var comment = await commentRepository
            .AsQueryable()
            .SingleOrDefaultAsync(c => c.Id == commentId, cancellationToken);
        
        if (comment is null)
        {
            logger.LogWarning("Comment with ID `{CommentId}` not found.", commentId);
            throw new NotFoundException($"Comment with ID '{commentId}' not found.");
        }
        
        logger.LogInformation("Comment with ID `{CommentId}` retrieved successfully.", commentId);
        
        return comment;
    }

    private Guid GetCurrentUserId() => authenticationService.GetUserIdFromAccessToken();
}