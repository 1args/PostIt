using PostIt.Domain.Enums;
using PostIt.Domain.Exceptions;
using PostIt.Domain.ValueObjects.User;

namespace PostIt.Domain.Entities;

public class User : Entity<Guid>
{
    private readonly List<Post> _posts = [];
    private readonly List<Comment> _comments = [];
    
    public Name Name { get; private set; }

    public Bio Bio { get; private set; }
    
    public IReadOnlyList<Post> Posts => _posts.AsReadOnly();

    public IReadOnlyList<Comment> Comments => _comments.AsReadOnly();
    
    public Email Email { get; private set; }
    
    public Password Password { get; private set; }

    public Role Role { get; private set; }

    public DateTime CreatedAt { get; private set; }
    
    public User Author { get; private set; } = null!;

    private User(Name name, Bio bio, Email email, Password password, Role role, DateTime createdAt)
    {
        Name = name;
        Bio = string.IsNullOrWhiteSpace(bio.ToString())
            ? Bio.Create("Empty")
            : bio ;
        Email = email;
        Password = password;
        Role = role;
        CreatedAt = createdAt;
    }
    
    public static User Create(
        Name name,
        Bio bio, 
        Email email, 
        Password password, 
        Role role,
        DateTime createdAt) =>
        new(name, bio, email, password, role, createdAt);

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
            throw new DomainException("Post not found", nameof(post));
        }
        _posts.Remove(post);
    }
}