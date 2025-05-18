using System.Text.RegularExpressions;
using PostIt.Domain.Exceptions;
using PostIt.Domain.Primitives;

namespace PostIt.Domain.ValueObjects;

/// <summary>
/// Represents the value object for a user's email.
/// </summary>
public class UserEmail : ValueObject
{
    /// <summary>Maximum length of email.</summary>
    public const int MaxLength = 320;
    
    /// <summary>The actual value.</summary>
    public string Value { get; }
    
    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private UserEmail() { }
    
    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private UserEmail(string email) 
    {
        if (!IsEmailValid(email))
        {
            throw new DomainException("Invalid email format.");
        }
        
        Value = email;
    }

    /// <summary>
    /// Factory method to create a new user instance.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns>A new instance of the <see cref="UserBio"/> class.</returns>
    /// <exception cref="DomainException">Thrown if email format is invalid.</exception>
    public static UserEmail Create(string email) => new(email);

    /// <summary>
    /// Validates the format of the given email.
    /// </summary>
    /// <param name="email">Email to check.</param>
    /// <returns><c>true</c> if valid; otherwise <c>false</c>.</returns>
    public static bool IsEmailValid(string email) 
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("Email cannot be empty.");
        }

        var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }
    
    /// <inheritdoc/>
    public override string ToString() => Value;
    
    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}