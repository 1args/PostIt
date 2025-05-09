using Microsoft.AspNetCore.Mvc;
using PostIt.Api.Extensions.Endpoints;
using PostIt.Application.Abstractions.Services;
using PostIt.Contracts.ApiContracts.Requests.Post;

namespace PostIt.Api.Endpoints;

public static class PostEndpoints
{
    public static IEndpointRouteBuilder MapPostEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/posts").WithTags("Posts")
            .RequireAuthorization();

        group.MapPost("/", CreatePostAsync)
            .WithRequestValidation<CreatePostRequest>();
        
        group.MapPut("{id:guid}", UpdatePostAsync)
            .WithRequestValidation<UpdatePostRequest>()
            .WithName(nameof(UpdatePostAsync));
        
        group.MapDelete("{id:guid}", DeletePostAsync)
            .WithName(nameof(DeletePostAsync));
        
        group.MapPost("{id:guid}/like/{authorId:guid}", LikePostAsync)
            .WithName("LikePost");
        
        group.MapDelete("{id:guid}/unlike/{authorId:guid}", UnlikePostAsync)
            .WithName("UnlikePost");
        
        group.MapPost("{id:guid}/view", ViewPostAsync)
            .WithName("ViewPost");

        group.MapPut("{id:guid}/visibility", ChangeVisibilityAsync)
            .WithRequestValidation<ChangePostVisibilityRequest>()
            .WithName("ChangePostVisibility");
        
        return endpoints;
    }

    private static async Task<IResult> CreatePostAsync(
        [FromBody] CreatePostRequest request,
        [FromServices] IPostService postService,
        CancellationToken cancellationToken)
    {
        await postService.CreatePostAsync(request, cancellationToken);

        return Results.NoContent();
    }

    private static async Task<IResult> UpdatePostAsync(
        [FromRoute] Guid id,
        [FromBody] UpdatePostRequest request,
        [FromServices] IPostService postService,
        CancellationToken cancellationToken)
    {
        await postService.UpdatePostAsync(id, request, cancellationToken);
        
        return Results.NoContent();
    }

    private static async Task<IResult> DeletePostAsync(
        [FromRoute] Guid id,
        [FromServices] IPostService postService,
        CancellationToken cancellationToken)
    {
        await postService.DeletePostAsync(id, cancellationToken);
        
        return Results.NoContent();
    }

    private static async Task<IResult> LikePostAsync(
        [FromRoute] Guid id,
        [FromServices] IPostService postService,
        CancellationToken cancellationToken)
    {
        await postService.LikePostAsync(id, cancellationToken);
        
        return Results.NoContent();
    }
    
    private static async Task<IResult> UnlikePostAsync(
        [FromRoute] Guid id,
        [FromServices] IPostService postService,
        CancellationToken cancellationToken)
    {
        await postService.UnlikePostAsync(id, cancellationToken);
        
        return Results.NoContent();
    }

    private static async Task<IResult> ViewPostAsync(
        [FromRoute] Guid id,
        [FromServices] IPostService postService,
        CancellationToken cancellationToken)
    {
        await postService.ViewPostAsync(id, cancellationToken);
        
        return Results.NoContent();
    }

    private static async Task<IResult> ChangeVisibilityAsync(
        [FromRoute] Guid id,
        [FromBody] ChangePostVisibilityRequest request,
        [FromServices] IPostService postService,
        CancellationToken cancellationToken)
    {
        await postService.ChangeVisibilityAsync(id, request, cancellationToken);
        
        return Results.NoContent();
    }
}