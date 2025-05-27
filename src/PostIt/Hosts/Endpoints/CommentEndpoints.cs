using Microsoft.AspNetCore.Mvc;
using PostIt.Api.Extensions.Endpoints;
using PostIt.Application.Abstractions.Services;
using PostIt.Contracts.ApiContracts.Requests.Comment;
using PostIt.Contracts.ApiContracts.Requests.Post;
using PostIt.Contracts.Models.Pagination;
using PostIt.Contracts.Models.Sorting;
using PostIt.Domain.Enums;

namespace PostIt.Api.Endpoints;

/// <summary>
/// Provides API endpoints for comment-related actions.
/// </summary>
public static class CommentEndpoints
{
    /// <summary>
    /// Maps all comment-related endpoints to the application's route builder.
    /// </summary>
    public static IEndpointRouteBuilder MapCommentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/comments").WithTags("Comments");

        group.MapPost("/", CreateCommentAsync)
            .WithRequestValidation<CreateCommentRequest>()
            .RequireAuthorization()
            .RequirePermissions(Permission.CreateComment);
        
        group.MapDelete("/{id:guid}", DeleteCommentAsync)
            .RequireAuthorization()
            .RequirePermissions(Permission.DeleteOwnComment, Permission.DeleteAnyComment);
        
        group.MapPost("/{id:guid}/likes", LikeCommentAsync)
            .RequireAuthorization()
            .RequirePermissions(Permission.LikeDislike);
        
        group.MapDelete("/{id:guid}/likes", UnlikeCommentAsync)
            .RequireAuthorization()
            .RequirePermissions(Permission.LikeDislike);
        
        group.MapGet("/{id:guid}", GetCommentsByPostAsync);
        
        return endpoints;
    }
    
    /// <summary>
    /// Creates a new comment.
    /// </summary>
    private static async Task<IResult> CreateCommentAsync(
        [FromBody] CreateCommentRequest request,
        [FromServices] ICommentService commentService,
        CancellationToken cancellationToken)
    {
        await commentService.CreateCommentAsync(request, cancellationToken);

        return Results.NoContent();
    }
    
    /// <summary>
    /// Deletes a comment by ID.
    /// </summary>
    private static async Task<IResult> DeleteCommentAsync(
        [FromRoute] Guid id,
        [FromServices] ICommentService commentService,
        CancellationToken cancellationToken)
    {
        await commentService.DeleteCommentAsync(id, cancellationToken);
        
        return Results.NoContent();
    }
    
    /// <summary>
    /// Deletes a comment by ID.
    /// </summary>
    private static async Task<IResult> LikeCommentAsync(
        [FromRoute] Guid id,
        [FromServices] ICommentService commentService,
        CancellationToken cancellationToken)
    {
        await commentService.LikeCommentAsync(id, cancellationToken);
        
        return Results.NoContent();
    }
    
    /// <summary>
    /// Unlikes a comment by ID.
    /// </summary>
    private static async Task<IResult> UnlikeCommentAsync(
        [FromRoute] Guid id,
        [FromServices] ICommentService commentService,
        CancellationToken cancellationToken)
    {
        await commentService.UnlikeCommentAsync(id, cancellationToken);
        
        return Results.NoContent();
    }
    
    /// <summary>
    /// Retrieves all comments for a specific post with optional sorting and pagination.
    /// </summary>
    private static async Task<IResult> GetCommentsByPostAsync(
        [FromRoute] Guid id,
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromQuery] string? sortBy,
        [FromQuery] bool isAscending,
        [FromServices] ICommentService commentService,
        CancellationToken cancellationToken)
    {
        var paginationParams = new PaginationParams
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        
        var sortParams = string.IsNullOrWhiteSpace(sortBy)
            ? null
            : new SortParams
            {
                Criteria = sortBy,
                IsAscending = isAscending
            };
        
        var comments = await commentService.PagingCommentsByPostAsync(
            id,
            sortParams,
            paginationParams,
            cancellationToken);
        
        return Results.Ok(comments);
    }
}