using PostIt.Domain.Exceptions;
using PostIt.Domain.Primitives;

namespace PostIt.Domain.ValueObjects;

/// <summary>
/// Represents the value object for a user's biography.
/// </summary>
public class UserBio : ValueObject
{
    /// <summary>Maximum length of the user bio.</summary>
    public const int MaxLength = 150;
    
    /// <summary>The actual value.</summary>
    public string Value { get; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private UserBio() { }
    
    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private UserBio(string bio)
    {
        if(string.IsNullOrWhiteSpace(bio))
        {
            Value = "Empty";
            return;
        }

        if (bio.Length > MaxLength)
        {
            throw new DomainException($"User bio must be no longer than {MaxLength} characters long.");
        }
        
        Value = bio;
    }

    /// <summary>
    /// Factory method to create a new user instance.
    /// </summary>
    /// <param name="bio">The biography string.</param>
    /// <returns>A new instance of the <see cref="UserBio"/> class.</returns>
    /// <exception cref="DomainException">Thrown if bio exceeds maximum length.</exception>
    public static UserBio Create(string bio) => new(bio);
    
    /// <summary>
    /// Checks whether the biography is empty or null.
    /// </summary>
    /// <returns><c>true</c> if bio is empty or null; otherwise, <c>false</c>.</returns>
    public bool IsEmpty() => string.IsNullOrWhiteSpace(Value);

    /// <inheritdoc/>
    public override string ToString() => Value;
    
    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}