namespace PostIt.Domain.ValueObjects.Author;

public class AuthorName
{
    public const int MinLength = 3;
    public const int MaxLength = 30;
    
    public string Value { get; }
    
    private AuthorName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Author name cannot be empty.", nameof(name));
        }

        Value = name.Length switch
        {
            < MinLength => throw new ArgumentException(
                $"Author name must be at least {MinLength} characters long.",
                nameof(name)),
            > MaxLength => throw new ArgumentException(
                $"Author name must be no longer than {MaxLength} characters.",
                nameof(name)),
            _ => name
        };
    }
    
    public static AuthorName Create(string name) => new(name);

    public override string ToString() => Value;
}