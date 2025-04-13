using PostIt.Domain.ValueObjects.Author;

namespace PostIt.Domain.Entities;

public class Author : Entity<Guid>
{
    private readonly List<Post> _posts = [];
    private readonly List<Comment> _comments = [];

    public Name Name { get; private set; }

    public Bio Bio { get; private set; }

    public Guid UserId { get; private set; }

    public User User { get; private set; } = null!;

    public IReadOnlyList<Post> Posts => _posts.AsReadOnly();

    public IReadOnlyList<Comment> Comments => _comments.AsReadOnly();

    private Author(Name name, Bio bio, Guid userId)
    {
        Name = name;
        Bio = bio;
        UserId = userId;
    }

    public static Author Create(Name name, Bio bio, Guid userId) =>
        new(name, bio, userId);

    public void UpdateBio(Bio bio)
    {
        if (Bio.Equals(bio))
        {
            return;
        }
        Bio = bio;
    }

    public void AddPost(Post post)
    {
        ArgumentNullException.ThrowIfNull(post);
        _posts.Add(post);
    }

    public void RemovePost(Post post)
    {
        if (!_posts.Contains(post))
        {
            
        }
        _posts.Remove(post);
    }
}