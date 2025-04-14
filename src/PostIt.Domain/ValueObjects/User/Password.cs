namespace PostIt.Domain.ValueObjects.User;

public class Password : ValueObject
{
    public const int MinLength = 8;
    public const int MaxLength = 16;
    
    public string Value { get; }

    private Password(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < MinLength)
        {
            throw new ArgumentException($"Password must be at least {MinLength} characters long.", 
                nameof(password));
        }

        if (password.Length > MaxLength)
        {
            throw new ArgumentException($"Password must be less than {MaxLength}.", 
                nameof(password));
        }
            
        Value = password;
    }

    public static Password Create(string password) => new(password);

    public override string ToString() => Value;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}