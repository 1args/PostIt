namespace PostIt.Domain.ValueObjects.Author;

public class AuthorBio
{
    public const int MaxLength = 150;
    
    public string Value { get; }

    private AuthorBio(string bio)
    {
        if(string.IsNullOrWhiteSpace(bio))
        {
            throw new ArgumentException("Author bio cannot be empty.", nameof(bio));
        }

        if (bio.Length > MaxLength)
        {
            throw new ArgumentException($"Author bio must be no longer than {MaxLength} characters.", 
                nameof(bio));
        }
        
        Value = bio;
    }

    private static AuthorBio Create(string bio) => new(bio);

    public override string ToString() => Value;
}