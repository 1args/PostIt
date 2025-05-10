using PostIt.Domain.Primitives;

namespace PostIt.Domain.Entities;

/// <summary>
/// Represents an email verification token entity.
/// </summary>
public sealed class EmailVerificationToken : Entity<Guid>, IAuditableEntity
{
    /// <summary>ID of the user associated with the token.</summary>
    public Guid UserId { get; private set; }
    
    /// <summary>Date when the comment was created.</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>Expiration date of token.</summary>
    public DateTime ExpirationDate  { get; private set; }

    /// <summary>User navigation property.</summary>
    public User User { get; private set; } = null!;

    /// <summary>
    /// Constructor for EF Core
    /// </summary>
    private EmailVerificationToken() { }
    
    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private EmailVerificationToken(Guid userId, DateTime createdAt)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAt = createdAt;
        ExpirationDate = CreatedAt.AddHours(24);
    }
    
    /// <summary>
    /// Factory method to create a new email verification token.
    /// </summary>
    /// <param name="userId">The ID of the user to associate the token with.</param>
    /// <param name="createdAt">The creation timestamp.</param>
    /// <returns>A new instance of <see cref="EmailVerificationToken"/>.</returns>
    public static EmailVerificationToken Create(Guid userId, DateTime createdAt) => new(userId, createdAt);
    
    /// <summary>
    /// Determines whether the token has expired.
    /// </summary>
    /// <returns><c>true</c> if the token has expired; otherwise, <c>false</c>.</returns>
    public bool IsExpired() => ExpirationDate < DateTime.UtcNow;
}