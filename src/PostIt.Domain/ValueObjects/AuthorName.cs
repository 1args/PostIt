namespace PostIt.Domain.ValueObjects;

public class AuthorName
{
    public const int MinLength = 3;
    public const int MaxLength = 30;
    
    public string Value { get; }
    
    private AuthorName(string name)
    {
        Value = name;
    }
}