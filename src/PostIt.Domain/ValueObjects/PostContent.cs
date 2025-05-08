using PostIt.Domain.Exceptions;
using PostIt.Domain.Primitives;

namespace PostIt.Domain.ValueObjects;

/// <summary>
/// Represents the value object for the content of a post.
/// </summary>
public class PostContent : ValueObject
{
    /// <summary>Minimum length of post content.</summary>
    public const int MinLength = 5;
    
    /// <summary>Maximum length of post content.</summary>
    public const int MaxLength = 4800;
    
    /// <summary>The actual value.</summary>
    public string Value { get; }

    public PostContent()
    {
        
    }
    
    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private PostContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new DomainException("Post content cannot be empty.");
        }
        
        Value = content.Length switch
        {
            < MinLength => throw new DomainException(
                $"Post content must be at least {MinLength} characters long."),
            > MaxLength => throw new DomainException(
                $"Post content must be no longer than {MaxLength} characters."),
            _ => content
        };
    }
    
    /// <summary>
    /// Factory method to create a new user instance.
    /// </summary>
    /// <param name="content">The content string.</param>
    /// <returns>A new instance of the <see cref="PostContent"/> class.</returns>
    /// <exception cref="DomainException">Thrown if the content is null, too short, or too long.</exception>
    public static PostContent Create(string content) => new(content);

    /// <inheritdoc/>
    public override string ToString() => Value;
    
    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}