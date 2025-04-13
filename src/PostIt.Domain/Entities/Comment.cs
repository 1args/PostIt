using PostIt.Domain.ValueObjects.Comment;

namespace PostIt.Domain.Entities;

public class Comment : Entity<Guid>
{
    public Text Text { get; private set; }

    public int Likes { get; private set; } = 0;
    
    public DateTime CreatedAt { get; private set; }
    
    public Guid AuthorId { get; private set; }
    
    public Author Author { get; private set; } = null!;

    public Guid PostId { get; private set; }
    
    public Post Post { get; private set; } = null!;

    private Comment(Text text, Guid authorId, Guid postId)
    {
        Text = text;
        AuthorId = authorId;
        PostId = postId;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void Like() => Likes++;

    public void Unlike()
    {
        if (Likes <= 0)
        {
            throw new ApplicationException("Likes cannot be negative.");
        }
        Likes--;
    }
}