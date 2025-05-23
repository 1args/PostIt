using PostIt.Domain.Exceptions;
using PostIt.Domain.Primitives;

namespace PostIt.Domain.ValueObjects;

/// <summary>
/// Represents the value object for a comment's text.
/// </summary>
public class CommentText : ValueObject
{
    /// <summary>Minimum allowed length for a comment.</summary>
    public const int MinLength = 3;
    
    /// <summary>Maximum allowed length for a comment.</summary>
    public const int MaxLength = 2400;
    
    /// <summary>The actual value.</summary>
    public string Value { get; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private CommentText() { }
    
    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private CommentText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new DomainException("Comment text cannot be empty.");
        }
        
        Value = text.Length switch
        {
            < MinLength => throw new DomainException(
                $"Comment text must be at least {MinLength} characters long."),
            > MaxLength => throw new DomainException(
                $"Comment text must be no longer than {MaxLength} characters."),
            _ => text
        };
    }
    
    /// <summary>
    /// Factory method to create a new user instance.
    /// </summary>
    /// <param name="text">Comment text to validate and assign.</param>
    /// <returns>A new instance of the <see cref="CommentText"/> class.</returns>
    /// <exception cref="DomainException">Thrown if text is null, empty, too short, or too long.</exception>
    public static CommentText Create(string text) => new CommentText(text);

    /// <inheritdoc/>
    public override string ToString() => Value;
    
    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}