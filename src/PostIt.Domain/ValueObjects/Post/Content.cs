namespace PostIt.Domain.ValueObjects.Post;

public class Content
{
    public const int MinLength = 20;
    public const int MaxLength = 4800;
    
    public string Value { get; }

    private Content(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Post content cannot be empty.", nameof(content));
        }
        
        Value = content.Length switch
        {
            < MinLength => throw new ArgumentException(
                $"Post content must be at least {MinLength} characters long.",
                nameof(content)),
            > MaxLength => throw new ArgumentException(
                $"Post content must be no longer than {MaxLength} characters.",
                nameof(content)),
            _ => content
        };
    }
    
    public static Content Create(string content) => new(content);

    public override string ToString() => Value;
}