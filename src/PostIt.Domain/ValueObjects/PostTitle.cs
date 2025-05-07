using PostIt.Domain.Exceptions;
using PostIt.Domain.Primitives;

namespace PostIt.Domain.ValueObjects;

/// <summary>
/// Represents the value object for the title for a post.
/// </summary>
public class PostTitle : ValueObject
{
    /// <summary>Minimum length of a title.</summary>
    public const int MinLength = 1;
    
    /// <summary>Maximum length of a title.</summary>
    public const int MaxLength = 100;
    
    /// <summary>The actual value.</summary>
    public string Value { get; }
    
    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private PostTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("Post title cannot be empty.");
        }

        Value = title.Length switch
        {
            < MinLength => throw new DomainException(
                $"Post title must be at least {MinLength} characters long."),
            > MaxLength => throw new DomainException(
                $"Post title must be no longer than {MaxLength} characters."),
            _ => title
        };
        
        Value = title;
    }

    /// <summary>
    /// Factory method to create a new user instance.
    /// </summary>
    /// <param name="title">Title text to assign.</param>
    /// <returns>A new instance of the <see cref="PostTitle"/> class.</returns>
    /// <exception cref="DomainException">Thrown if title is null, too short, or too long.</exception>
    public static PostTitle Create(string title) => new(title);

    /// <inheritdoc/>
    public override string ToString() => Value;
    
    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}