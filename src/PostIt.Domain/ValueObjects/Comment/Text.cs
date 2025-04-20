using PostIt.Domain.Exceptions;

namespace PostIt.Domain.ValueObjects.Comment;

public class Text : ValueObject
{
    public const int MinLength = 3;
    public const int MaxLength = 2400;
    
    public string Value { get; }

    private Text() { }
    
    private Text(string text)
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
    
    public static Text Create(string text) => new Text(text);

    public override string ToString() => Value;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}