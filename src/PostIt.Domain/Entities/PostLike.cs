using PostIt.Domain.Exceptions;

namespace PostIt.Domain.Entities;

public class PostLike
{
    public Guid PostId { get; private set; }
    
    public Post Post { get; private set; } = null!;
    
    public Guid AuthorId { get; private set; }

    public User Author { get; private set; } = null!;
    
    private PostLike() { }

    private PostLike(Guid postId, Guid authorId)
    {
        if (postId == Guid.Empty)
        {
            throw new DomainException("Comment ID cannot be empty", nameof(postId));
        }

        if (authorId == Guid.Empty)
        {
            throw new DomainException("Author ID cannot be empty", nameof(authorId));
        }
        
        PostId = postId;
        AuthorId = authorId;
    }
    
    public static PostLike Create(Guid postId, Guid authorId) => new(postId, authorId);
}