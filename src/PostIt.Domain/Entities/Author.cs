using PostIt.Domain.ValueObjects;

namespace PostIt.Domain.Entities;

public class Author : Entity<Guid>
{
    private readonly List<Post> _posts = [];
    private readonly List<Comment> _comments = [];
    
    public AuthorName Name { get; private set; }

    public AuthorBio Bio { get; private set; }

    public Guid UserId { get; private set; }

    public User User { get; private set; } = null!;
    
    public IReadOnlyList<Post> Posts => _posts;
    
    public IReadOnlyList<Comment> Comments => _comments;

    private Author(AuthorName name, AuthorBio bio)
    {
        Name = name;
        Bio = bio;
    }
    
    public static Author Create(AuthorName name, AuthorBio bio) => new(name, bio);
}