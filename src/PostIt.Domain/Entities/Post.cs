using PostIt.Domain.Enums;
using PostIt.Domain.ValueObjects.Post;

namespace PostIt.Domain.Entities;

public class Post : Entity<Guid>
{
    private readonly HashSet<Guid> _likedUserIds = [];
    private readonly List<Comment> _comments = [];
    
    public Title Title { get; private set; } 
    
    public Content Content { get; private set; }

    public int Views { get; private set; } = 0;
    
    public int Likes { get; private set; } = 0;
    
    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public bool WasUpdated => UpdatedAt.HasValue;

    public Visibility Visibility { get; private set; }

    public IReadOnlyCollection<Guid> LikedUserIds => _likedUserIds;
    
    public IReadOnlyList<Comment> Comments => _comments.AsReadOnly();

    public Guid AuthorId { get; private set; }
    
    public Author Author { get; private set; } = null!;

    private Post(
        Title title, 
        Content content,
        Guid authorId, 
        Visibility visibility = Visibility.Public)
    {
        Title = title;
        Content = content;
        AuthorId = authorId;
        CreatedAt = DateTime.UtcNow;
        Visibility = visibility;
    }
    
    public static Post Create(
        Title title, 
        Content content,
        Guid authorId,
        Visibility visibility = Visibility.Public) =>
            new(title, content, authorId, visibility);

    public void Like(Guid userId)
    {
        if (!_likedUserIds.Add(userId))
        {
            throw new ArgumentException($"User with id {userId} already liked this post.");
        }
        Likes++;
    }

    public void Unlike(Guid userId)
    {
        if (!_likedUserIds.Remove(userId))
        {
            throw new ArgumentException($"User with id {userId} not liked this post.");
        }
        Likes--;
    }
    
    public void View() => Views++;

    public void AddComment(Comment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);
        _comments.Add(comment);
    }

    public void RemoveComment(Comment comment)
    {
        if (!_comments.Contains(comment))
        {
            throw new ArgumentException("Comment not found", nameof(comment));
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