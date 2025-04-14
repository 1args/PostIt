namespace PostIt.Domain.Entities;

public class CommentLike
{
    public Guid CommentId { get; private set; }
    
    public Comment Comment { get; private set; } = null!;
    
    public Guid AuthorId { get; private set; }

    public Author Author { get; private set; } = null!;

    private CommentLike(Guid postId, Guid authorId)
    {
        CommentId = postId;
        AuthorId = authorId;
    }
    
    public static CommentLike Create(Guid commentId, Guid authorId) => new(commentId, authorId);
}