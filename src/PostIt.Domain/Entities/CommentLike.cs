namespace PostIt.Domain.Entities;

/// <summary>
/// Represents a like for a comment.
/// </summary>
public class CommentLike
{
    /// <summary>ID of the comment to which the like was added.</summary>
    public Guid CommentId { get; private set; }
    
    /// <summary>The ID of the user who liked the post.</summary>
    public Guid AuthorId { get; private set; }
    
    /// <summary>Comment navigation property.</summary>
    public Comment Comment { get; private set; } = null!;
    
    /// <summary>Author navigation property.</summary>
    public User Author { get; private set; } = null!;

    /// <summary>
    /// Constructor for EF Core
    /// </summary>
    private CommentLike() { }
    
    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private CommentLike(Guid commentId, Guid authorId)
    {
        CommentId = commentId;
        AuthorId = authorId;
    }
    
    /// <summary>
    /// Factory method to create a new comment like.
    /// </summary>
    /// <param name="commentId">The ID of the comment to like.</param>
    /// <param name="authorId">The ID of the author who likes the comment.</param>
    /// <returns>A new instance of <see cref="CommentLike"/>.</returns>
    public static CommentLike Create(Guid commentId, Guid authorId) => new(commentId, authorId);
}