using PostIt.Domain.Exceptions;
using PostIt.Domain.Primitives;
using PostIt.Domain.ValueObjects;

namespace PostIt.Domain.Entities;

/// <summary>
/// Represents a comment entity.
/// </summary>
public class Comment : Entity<Guid>, IAuditableEntity, IAuthoredEntity
{
    /// <summary>Text content of the comment.</summary>
    public CommentText Text { get; private set; }
    
    /// <summary>Date when the comment was created.</summary>
    public DateTime CreatedAt { get; private set; }
    
    private readonly List<CommentLike> _likes;
    
    /// <summary>Read-only list of likes.</summary>
    public IReadOnlyList<CommentLike> Likes => _likes.AsReadOnly();
    
    /// <summary>Author's user ID.</summary>
    public Guid AuthorId { get; private set; }

    /// <summary>ID of the post to which this comment belongs.</summary>
    public Guid PostId { get; private set; }
    
    /// <summary>Author navigation property.</summary>
    public User Author { get; private set; } = null!;
    
    /// <summary>Post navigation property.</summary>
    public Post Post { get; private set; } = null!;

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private Comment() { }

    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private Comment(
        CommentText text,
        Guid authorId, 
        Guid postId, 
        DateTime createdAt)
    {
        Text = text;
        AuthorId = authorId;
        PostId = postId;
        CreatedAt = createdAt;
    }
    
    /// <summary>
    /// Factory method to create a new comment.
    /// </summary>
    /// <param name="text">The text of the comment.</param>
    /// <param name="authorId">The ID of the comment author.</param>
    /// <param name="postId">The ID of the post the comment belongs to.</param>
    /// <param name="createdAt">The creation timestamp.</param>
    /// <returns>A new instance of the <see cref="Comment"/> class.</returns>
    public static Comment Create(CommentText text, Guid authorId, Guid postId, DateTime createdAt) =>
        new(text, authorId, postId, createdAt);

    /// <summary>
    /// Adds a like to the comment by a user.
    /// </summary>
    /// <param name="userId">The ID of the user liking the comment.</param>
    /// <exception cref="DomainException">
    /// Thrown if the user already liked the comment.
    /// </exception>
    public void Like(Guid userId)
    {
        if(_likes.Any(l => l.AuthorId == userId))
        {
            throw new DomainException($"User with ID '{userId}' already liked this comment.");
        }
        _likes.Add(CommentLike.Create(Id, userId));
    }

    /// <summary>
    /// Removes a like from the comment by a user.
    /// </summary>
    /// <param name="userId">The ID of the user unliking the comment.</param>
    /// <exception cref="DomainException">
    /// Thrown if the user hasn't liked the comment.
    /// </exception>
    public void Unlike(Guid userId)
    {
        var like = _likes.FirstOrDefault(l => l.AuthorId == userId);
        
        if (like is null)
        {
            throw new DomainException($"User with ID '{userId}' not liked this comment.");
        }
        _likes.Remove(like);
    }
}