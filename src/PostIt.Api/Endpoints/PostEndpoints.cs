using Microsoft.AspNetCore.Mvc;
using PostIt.Api.Extensions.Endpoints;
using PostIt.Application.Abstractions.Services;
using PostIt.Contracts.ApiContracts.Requests.Post;
using PostIt.Contracts.Models.Pagination;
using PostIt.Contracts.Models.Sorting;

namespace PostIt.Api.Endpoints;

/// <summary>
/// Provides API endpoints for post-related actions.
/// </summary>
public static class PostEndpoints
{
    /// <summary>
    /// Maps all post-related endpoints to the application's route builder.
    /// </summary>
    public static IEndpointRouteBuilder MapPostEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/posts").WithTags("Posts");

        group.MapPost("/", CreatePostAsync)
            .WithRequestValidation<CreatePostRequest>()
            .RequireAuthorization();
        
        group.MapPut("{id:guid}", UpdatePostAsync)
            .WithRequestValidation<UpdatePostRequest>()
            .WithName(nameof(UpdatePostAsync))
            .RequireAuthorization();
        
        group.MapDelete("{id:guid}", DeletePostAsync)
            .WithName(nameof(DeletePostAsync))
            .RequireAuthorization();
        
        group.MapPost("{id:guid}/like", LikePostAsync)
            .WithName("LikePost")
            .RequireAuthorization();
        
        group.MapDelete("{id:guid}/unlike", UnlikePostAsync)
            .WithName("UnlikePost")
            .RequireAuthorization();
        
        group.MapPost("{id:guid}/view", ViewPostAsync)
            .WithName("ViewPost")
            .RequireAuthorization();

        group.MapPut("{id:guid}/visibility", ChangeVisibilityAsync)
            .WithRequestValidation<ChangePostVisibilityRequest>()
            .WithName("ChangePostVisibility")
            .RequireAuthorization();

        group.MapGet("/", GetAllPostsAsync)
            .WithName("GetAllPosts");
        
        return endpoints;
    }

    /// <summary>
    /// Creates a new post.
    /// </summary>
    private static async Task<IResult> CreatePostAsync(
        [FromBody] CreatePostRequest request,
        [FromServices] IPostService postService,
        CancellationToken cancellationToken)
    {
        await postService.CreatePostAsync(request, cancellationToken);

        return Results.NoContent();
    }

    /// <summary>
    /// Updates an existing post by ID.
    /// </summary>
    private static async Task<IResult> UpdatePostAsync(
        [FromRoute] Guid id,
        [FromBody] UpdatePostRequest request,
        [FromServices] IPostService postService,
        CancellationToken cancellationToken)
    {
        await postService.UpdatePostAsync(id, request, cancellationToken);
        
        return Results.NoContent();
    }

    /// <summary>
    /// Deletes a post by ID.
    /// </summary>
    private static async Task<IResult> DeletePostAsync(
        [FromRoute] Guid id,
        [FromServices] IPostService postService,
        CancellationToken cancellationToken)
    {
        await postService.DeletePostAsync(id, cancellationToken);
        return Results.NoContent();
    }

    /// <summary>
    /// Likes a post by ID.
    /// </summary>
    private static async Task<IResult> LikePostAsync(
        [FromRoute] Guid id,
        [FromServices] IPostService postService,
        CancellationToken cancellationToken)
    {
        await postService.LikePostAsync(id, cancellationToken);
        return Results.NoContent();
    }

    /// <summary>
    /// Removes like from a post by ID.
    /// </summary>
    private static async Task<IResult> UnlikePostAsync(
        [FromRoute] Guid id,
        [FromServices] IPostService postService,
        CancellationToken cancellationToken)
    {
        await postService.UnlikePostAsync(id, cancellationToken);
        return Results.NoContent();
    }

    /// <summary>
    /// Marks a post as viewed by ID.
    /// </summary>
    private static async Task<IResult> ViewPostAsync(
        [FromRoute] Guid id,
        [FromServices] IPostService postService,
        CancellationToken cancellationToken)
    {
        await postService.ViewPostAsync(id, cancellationToken);
        return Results.NoContent();
    }

    /// <summary>
    /// Changes the visibility of a post by ID.
    /// </summary>
    private static async Task<IResult> ChangeVisibilityAsync(
        [FromRoute] Guid id,
        [FromBody] ChangePostVisibilityRequest request,
        [FromServices] IPostService postService,
        CancellationToken cancellationToken)
    {
        await postService.ChangeVisibilityAsync(id, request, cancellationToken);
        return Results.NoContent();
    }

    /// <summary>
    /// Retrieves all posts with optional sorting and pagination.
    /// </summary>
    private static async Task<IResult> GetAllPostsAsync(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromQuery] string? sortBy,
        [FromQuery] bool isAscending,
        [FromServices] IPostService postService,
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

        var posts = await postService.GetAllPosts(sortParams, paginationParams, cancellationToken);

        return Results.Ok(posts);
    }
}