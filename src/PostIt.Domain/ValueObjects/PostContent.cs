using PostIt.Domain.Exceptions;
using PostIt.Domain.Primitives;

namespace PostIt.Domain.ValueObjects;

public class PostContent : ValueObject
{
    public const int MinLength = 5;
    public const int MaxLength = 4800;
    
    public string Value { get; }
    
    private PostContent() { }
    
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
    
    public static PostContent Create(string content) => new(content);

    public override string ToString() => Value;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}