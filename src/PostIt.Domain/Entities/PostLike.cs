namespace PostIt.Domain.Entities;

public class PostLike
{
    public Guid PostId { get; private set; }
    
    public Post Post { get; private set; } = null!;
    
    public Guid AuthorId { get; private set; }

    public User Author { get; private set; } = null!;

    private PostLike(Guid postId, Guid authorId)
    {
        PostId = postId;
        AuthorId = authorId;
    }
    
    public static PostLike Create(Guid postId, Guid authorId) => new(postId, authorId);
}