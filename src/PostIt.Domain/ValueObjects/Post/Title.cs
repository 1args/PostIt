using PostIt.Domain.Exceptions;

namespace PostIt.Domain.ValueObjects.Post;

public class Title : ValueObject
{
    public const int MinLength = 1;
    public const int MaxLength = 100;
    
    public string Value { get; set; }
    
    private Title() { }

    private Title(string title)
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

    public static Title Create(string title) => new(title);

    public override string ToString() => Value;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}