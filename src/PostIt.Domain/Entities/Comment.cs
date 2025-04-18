using PostIt.Domain.Exceptions;
using PostIt.Domain.ValueObjects.Comment;

namespace PostIt.Domain.Entities;

public class Comment : Entity<Guid>
{
    private readonly List<CommentLike> _likes = [];
    
    public Text Text { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public IReadOnlyList<CommentLike> Likes => _likes;
    
    public Guid AuthorId { get; private set; }
    
    public User Author { get; private set; } = null!;

    public Guid PostId { get; private set; }
    
    public Post Post { get; private set; } = null!;

    private Comment(Text text, Guid authorId, Guid postId, DateTime createdAt)
    {
        if (createdAt > DateTime.Now)
        {
            throw new DomainException("Creation date cannot be in the future.", nameof(createdAt));
        }

        Text = text;
        AuthorId = authorId;
        PostId = postId;
        CreatedAt = createdAt;
    }
    
    public static Comment Create(Text text, Guid authorId, Guid postId, DateTime createdAt) =>
        new(text, authorId, postId, createdAt);

    public void Like(Guid userId)
    {
        if(_likes.Any(l => l.AuthorId == userId))
        {
            throw new DomainException($"User with id {userId} already liked this comment.");
        }
        _likes.Add(CommentLike.Create(Id, userId));
    }

    public void Unlike(Guid userId)
    {
        var like = _likes.FirstOrDefault(l => l.AuthorId == userId);
        
        if (like is null)
        {
            throw new DomainException($"User with id {userId} not liked this comment.",
                nameof(userId));
        }
        _likes.Remove(like);
    }
}