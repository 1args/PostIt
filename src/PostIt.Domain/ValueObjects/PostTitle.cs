namespace PostIt.Domain.ValueObjects;

public class PostTitle
{
    public const int MinLength = 1;
    public const int MaxLength = 100;
    
    public string Value { get; set; }

    private PostTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Post title cannot be empty", nameof(title));
        }

        Value = title.Length switch
        {
            < MinLength => throw new ArgumentException(
                $"Post title must be at least {MinLength} characters long",
                nameof(title)),
            > MaxLength => throw new ArgumentException(
                $"Post title must be no longer than {MaxLength} characters.",
                nameof(title)),
            _ => title
        };
        
        Value = title;
    }

    private static PostTitle Create(string title) => new(title);

    public override string ToString() => Value;
}