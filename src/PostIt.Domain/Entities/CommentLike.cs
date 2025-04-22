using PostIt.Domain.Exceptions;

namespace PostIt.Domain.Entities;

public class CommentLike
{
    public Guid CommentId { get; private set; }
    
    public Comment Comment { get; private set; } = null!;
    
    public Guid AuthorId { get; private set; }

    public User Author { get; private set; } = null!;
    
    private CommentLike() { }

    private CommentLike(Guid commentId, Guid authorId)
    {
        CommentId = commentId;
        AuthorId = authorId;
    }
    
    public static CommentLike Create(Guid commentId, Guid authorId) => new(commentId, authorId);
}