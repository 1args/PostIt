using PostIt.Domain.Enums;
using PostIt.Domain.Exceptions;
using PostIt.Domain.ValueObjects.User;

namespace PostIt.Domain.Entities;

public class User : Entity<Guid>
{
    public Name Name { get; private set; }

    public Bio Bio { get; private set; }

    public int PostsCount { get; private set; }
    
    public int CommentsCount { get; private set; }
    
    public Email Email { get; private set; }

    public Role Role { get; private set; }

    public DateTime CreatedAt { get; private set; }
    
    public Guid? RecognizedUserId { get; private set; }
    
    public RecognizedUser? RecognizedUser { get; private set; }
    
    private User() { }

    private User(Name name, Bio bio, Email email, Role role, DateTime createdAt)
    {
        if (createdAt > DateTime.UtcNow)
        {
            throw new DomainException("Creation date cannot be in the future.", nameof(createdAt));
        }
        
        Name = name;
        Bio = bio.IsEmpty() ? Bio.Create("Empty") : bio;
        Email = email;
        Role = role;
        CreatedAt = createdAt;
    }
    
    public static User Create(
        Name name,
        Bio bio, 
        Email email, 
        Role role,
        DateTime createdAt) =>
        new(name, bio, email, role, createdAt);

    public void UpdateBio(Bio bio)
    {
        if (Bio.Equals(bio))
        {
            return;
        }
        Bio = bio;
    }
    
    public void IncrementPostsCount() => PostsCount++;
    
    public void DecrementPostsCount()
    {
        if (PostsCount <= 0) throw new DomainException("Posts count cannot be negative.");
        PostsCount--;
    }
    
    public void IncrementCommentsCount() => CommentsCount++;
    
    public void DecrementCommentsCount()
    {
        if (CommentsCount <= 0) throw new DomainException("Comment count cannot be negative.");
        PostsCount--;
    }
    
}