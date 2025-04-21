using Microsoft.AspNetCore.Mvc;
using PostIt.Application.Abstractions.Services;
using PostIt.Contracts.ApiContracts.Requests.Comment;

namespace PostIt.Api.Endpoints;

public static class CommentEndpoints
{
    public static IEndpointRouteBuilder MapCommentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/comments/").WithTags("Comments");

        group.MapPost(" ", CreateCommentAsync).WithName("CreateComment");
        group.MapDelete("{id:guid}", DeleteCommentAsync).WithName("DeleteComment");
        group.MapPost("{id:guid}/like/{authorId:guid}", LikeCommentAsync).WithName("LikeComment");
        group.MapDelete("{id:guid}/like/{authorId:guid}", UnlikeCommentAsync).WithName("UnlikeComment");
        group.MapGet("post/{id:guid}", GetCommentsByPostAsync).WithName("GetCommentsByPost");
        
        return endpoints;
    }
    
    private static async Task<IResult> CreateCommentAsync(
        [FromBody] CreateCommentRequest request,
        [FromServices] ICommentService commentService,
        CancellationToken cancellationToken)
    {
        await commentService.CreateCommentAsync(request, cancellationToken);

        return Results.NoContent();
    }
    
    private static async Task<IResult> DeleteCommentAsync(
        [FromRoute] Guid id,
        [FromServices] ICommentService commentService,
        CancellationToken cancellationToken)
    {
        await commentService.DeleteCommentAsync(id, cancellationToken);
        
        return Results.NoContent();
    }
    
    private static async Task<IResult> LikeCommentAsync(
        [FromRoute] Guid id,
        [FromRoute] Guid authorId,
        [FromServices] ICommentService commentService,
        CancellationToken cancellationToken)
    {
        await commentService.LikeCommentAsync(id, authorId, cancellationToken);
        
        return Results.NoContent();
    }
    
    private static async Task<IResult> UnlikeCommentAsync(
        [FromRoute] Guid id,
        [FromRoute] Guid authorId,
        [FromServices] ICommentService commentService,
        CancellationToken cancellationToken)
    {
        await commentService.UnlikeCommentAsync(id, authorId, cancellationToken);
        
        return Results.NoContent();
    }

    private static async Task<IResult> GetCommentsByPostAsync(
        [FromRoute] Guid id,
        [FromServices] ICommentService commentService,
        CancellationToken cancellationToken)
    {
        var comments = await commentService.GetCommentsByPostAsync(id, cancellationToken);
        
        return Results.Ok(comments);
    }
}