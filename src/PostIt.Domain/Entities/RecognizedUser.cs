using PostIt.Domain.ValueObjects.User;

namespace PostIt.Domain.Entities;

public class RecognizedUser : Entity<Guid>
{
    public Guid UserId { get; private set; }

    public Salt Salt { get; private set; } 

    public Password PasswordHash { get; private set; } 
    
    public User User { get; private set; } = null!;

    private RecognizedUser(Guid userId, Salt salt, Password passwordHash)
    {
        UserId = userId;
        Salt = salt;
        PasswordHash = passwordHash;
    }
    
    public static RecognizedUser Create(Guid userId, Salt salt, Password passwordHash) => 
        new(userId, salt, passwordHash);
    
}