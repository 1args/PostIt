using PostIt.Domain.Exceptions;
using PostIt.Domain.Primitives;

namespace PostIt.Domain.ValueObjects;

/// <summary>
/// Represents a value object for a user's display name.
/// </summary>
public class UserName : ValueObject
{
    /// <summary>Minimum allowed length for the username.</summary>
    public const int MinLength = 3;
    
    /// <summary>Maximum allowed length for the username.</summary>
    public const int MaxLength = 30;
    
    /// <summary>The actual value.</summary>
    public string Value { get; }
    
    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private UserName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("User name cannot be empty.");
        }

        Value = name.Length switch
        {
            < MinLength => throw new DomainException(
                $"User name must be at least {MinLength} characters long."),
            > MaxLength => throw new DomainException(
                $"User name must be no longer than {MaxLength} characters long."),
            _ => name
        };
    }
    
    /// <summary>
    /// Factory method to create a new user instance.
    /// </summary>
    /// <param name="name">The user's name string.</param>
    /// <returns>A new instance of the <see cref="PostTitle"/> class.</returns>
    /// <exception cref="DomainException">If name is empty, too short, or too long.</exception>
    public static UserName Create(string name) => new(name);

    /// <inheritdoc/>
    public override string ToString() => Value;
    
    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}