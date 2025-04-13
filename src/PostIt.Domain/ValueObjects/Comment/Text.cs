namespace PostIt.Domain.ValueObjects.Comment;

public class Text
{
    public const int MinLength = 3;
    public const int MaxLength = 2400;
    
    public string Value { get; }
    
    private Text(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Comment text cannot be empty.", nameof(text));
        }
        
        Value = text.Length switch
        {
            < MinLength => throw new ArgumentException(
                $"Comment text must be at least {MinLength} characters long.",
                nameof(text)),
            > MaxLength => throw new ArgumentException(
                $"Comment text must be no longer than {MaxLength} characters.",
                nameof(text)),
            _ => text
        };
    }
    
    public static Text Create(string text) => new Text(text);

    public override string ToString() => Value;
}