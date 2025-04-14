namespace PostIt.Domain.Entities;

public class PostLike
{
    public Guid PostId { get; private set; }
    
    public Post Post { get; private set; } = null!;
    
    public Guid AuthorId { get; private set; }

    public Author Author { get; private set; } = null!;

    private PostLike(Guid postId, Guid userId)
    {
        PostId = postId;
        AuthorId = userId;
    }
    
    public static PostLike Create(Guid postId, Guid userId) => new(postId, userId);
}