using Microsoft.AspNetCore.Mvc;
using PostIt.Api.Extensions.Endpoints;
using PostIt.Application.Abstractions.Services;
using PostIt.Contracts.ApiContracts.Requests.Comment;
using PostIt.Contracts.ApiContracts.Requests.Post;
using PostIt.Contracts.Models.Pagination;
using PostIt.Contracts.Models.Sorting;

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
            .WithRequestValidation<CreatePostRequest>()
            .WithName("CreateComment");
        
        group.MapDelete("{id:guid}", DeleteCommentAsync)
            .WithName("DeleteComment");
        
        group.MapPost("{id:guid}/like/{authorId:guid}", LikeCommentAsync)
            .WithName("LikeComment");
        
        group.MapDelete("{id:guid}/like/{authorId:guid}", UnlikeCommentAsync)
            .WithName("UnlikeComment");
        
        group.MapGet("{id:guid}", GetCommentsByPostAsync)
            .WithName("GetCommentsByPost");
        
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
        
        var comments = await commentService.GetCommentsByPostAsync(
            id,
            sortParams,
            paginationParams,
            cancellationToken);
        
        return Results.Ok(comments);
    }
}