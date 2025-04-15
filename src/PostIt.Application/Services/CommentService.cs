using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostIt.Application.Contracts.Requests.Comment;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects.Comment;
using PostIt.Infrastructure.Configuration.Repositories;

namespace PostIt.Application.Services;

public class CommentService(
    IRepository<Comment> commentRepository,
    IRepository<Post> postRepository,
    ILogger<CommentService> logger)
{
    public async Task<Guid> CreateComment(
        CreateCommentRequest request,
        CancellationToken cancellationToken = default)
    {
        var text = Text.Create(request.Text);

        var post = await postRepository
            .Where(p => p.Id == request.PostId)
            .FirstOrDefaultAsync(cancellationToken);

        if (post is null)
        {
            throw new InvalidOperationException($"Post with ID '{request.PostId}' not found.");
        }

        var comment = Comment.Create(text, request.AuthorId, post.Id, DateTime.UtcNow);
        
        await commentRepository.AddAsync(comment, cancellationToken);
        return comment.Id;
    }
}