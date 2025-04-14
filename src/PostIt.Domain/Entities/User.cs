using PostIt.Domain.Enums;
using PostIt.Domain.ValueObjects.User;

namespace PostIt.Domain.Entities;

public class User : Entity<Guid>
{
    public Email Email { get; private set; }
    
    public Password Password { get; private set; }

    public Role Role { get; private set; }

    public DateTime CreatedAt { get; private set; }
    
    public Author Author { get; private set; } = null!;

    private User(Email email, Password password, Role role, DateTime createdAt)
    {
        if (createdAt > DateTime.UtcNow)
        {
            throw new ArgumentException("Creation date cannot be in the future.");
        }
        Email = email;
        Password = password;
        Role = role;
        CreatedAt = createdAt;
    }
    
    public static User Create(Email email, Password password, Role role, DateTime createdAt) =>
        new(email, password, role, createdAt);
}