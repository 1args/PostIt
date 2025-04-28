namespace PostIt.Domain.Entities;

public sealed class EmailVerificationToken : Entity<Guid>
{
    public Guid UserId { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public DateTime ExpirationDate  { get; private set; }

    public User User { get; private set; } = null!;

    private EmailVerificationToken() { }

    private EmailVerificationToken(Guid userId, DateTime createdAt)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAt = createdAt;
        ExpirationDate = CreatedAt.AddHours(24);
    }
    
    public static EmailVerificationToken Create(Guid userId, DateTime createdAt) => new(userId, createdAt);
    
    public bool IsExpired() => ExpirationDate < DateTime.UtcNow;
}