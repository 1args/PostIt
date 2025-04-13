using PostIt.Domain.Enums;
using PostIt.Domain.ValueObjects.User;

namespace PostIt.Domain.Entities;

public class User : Entity<Guid>
{
    public Email Email { get; private set; }
    
    public Password Password { get; private set; }

    public Role Role { get; private set; }

    public DateTime CreatedAt { get; private set; } 

    private User(Email email, Password password, Role role)
    {
        Email = email;
        Password = password;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }
    
    public static User Create(Email email, Password password, Role role) =>
        new(email, password, role);
}