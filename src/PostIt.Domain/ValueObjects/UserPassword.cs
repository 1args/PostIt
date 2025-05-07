using PostIt.Domain.Exceptions;
using PostIt.Domain.Primitives;

namespace PostIt.Domain.ValueObjects;

/// <summary>
/// Represents the value object for a user's password.
/// </summary>
public class UserPassword : ValueObject
{
    /// <summary>Minimum required length for password.</summary>
    public const int MinLength = 8;
    
    /// <summary>Maximum allowed length for password.</summary>
    public const int MaxLength = 100;
    
    /// <summary>The actual value.</summary>
    public string Value { get; }
    
    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private UserPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new DomainException("Password cannot be empty.");
        }
        
        Value = password;
    }
    
    /// <summary>
    /// Factory method to create a new user instance.
    /// </summary>
    /// <param name="password">Password value.</param>
    /// <returns>A new instance of the <see cref="UserPassword"/> class.</returns>
    /// <exception cref="DomainException">If password is empty or whitespace.</exception>
    public static UserPassword Create(string password) => new(password);

    /// <summary>
    /// Determines whether the given password is strong.
    /// </summary>
    /// <param name="password">Password to check.</param>
    /// <returns><c>true</c> if the password is considered strong; otherwise, <c>false</c>.</returns>
    public static bool IsPasswordStrong(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }
        if (password.Length is < MinLength or > MaxLength)
        {
            return false;
        }
        
        var hasUpper = password.Any(char.IsUpper);
        var hasLower = password.Any(char.IsLower);
        var hasDigit = password.Any(char.IsDigit);
        var hasSymbol = password.Any(ch => char.IsSymbol(ch) || char.IsPunctuation(ch));
        
        return hasUpper && hasLower && hasDigit && hasSymbol;
    }
    
    /// <inheritdoc/>
    public override string ToString() => Value;
    
    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}