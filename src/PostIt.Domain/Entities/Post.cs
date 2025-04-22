using PostIt.Domain.Enums;
using PostIt.Domain.Exceptions;
using PostIt.Domain.ValueObjects.Post;

namespace PostIt.Domain.Entities;

public class Post : Entity<Guid>
{
    private readonly List<PostLike> _likes = [];
    private readonly List<Comment> _comments = [];
    
    public Title Title { get; private set; } 
    
    public Content Content { get; private set; }

    public int ViewCount { get; private set; }
    
    public int LikesCount { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public bool WasUpdated => UpdatedAt.HasValue;

    public Visibility Visibility { get; private set; }

    public IReadOnlyList<PostLike> Likes => _likes.AsReadOnly();
    
    public IReadOnlyList<Comment> Comments => _comments.AsReadOnly();

    public Guid AuthorId { get; private set; }
    
    public User Author { get; private set; } = null!;

    public Post() { }

    private Post(
        Title title, 
        Content content,
        Guid authorId, 
        DateTime createdAt,
        Visibility visibility = Visibility.Public)
    {
        if (createdAt > DateTime.UtcNow)
        {
            throw new DomainException("Creation date cannot be in the future.", nameof(createdAt));
        }

        Title = title;
        Content = content;
        AuthorId = authorId;
        CreatedAt = createdAt;
        Visibility = visibility;
    }
    
    public static Post Create(
        Title title, 
        Content content,
        Guid authorId,
        DateTime createdAt,
        Visibility visibility = Visibility.Public) =>
            new(title, content, authorId, createdAt, visibility);

    public void Like(Guid userId)
    {
        if(_likes.Any(l => l.AuthorId == userId))
        {
            throw new DomainException($"User with ID {userId} already liked this post.");
        }
        _likes.Add(PostLike.Create(Id, userId));
        LikesCount++;
    }

    public void Unlike(Guid userId)
    {
        var like = _likes.FirstOrDefault(l => l.AuthorId == userId);
        
        if (like is null)
        {
            throw new DomainException($"User with ID {userId} not liked this post.");
        }
        _likes.Remove(like);
        LikesCount--;
    }
    
    public void View() => ViewCount++;

    public void AddComment(Comment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);
        _comments.Add(comment);
    }

    public void RemoveComment(Comment comment)
    {
        if (!_comments.Contains(comment))
        {
            throw new DomainException($"Comment with ID {comment.Id} not found");
        }
        _comments.Remove(comment);
    }
    
    public void UpdateContent(Title title, Content content)
    {
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(content);

        if (Title.Equals(title) && Content.Equals(content))
        {
            return;
        }
        
        Title = title;
        Content = content;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetVisibility(Visibility visibility) => Visibility = visibility;

    public bool IsVisibleToUser(Guid userId) =>
        Visibility == Visibility.Public || AuthorId == userId;
}