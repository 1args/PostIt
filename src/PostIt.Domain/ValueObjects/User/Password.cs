using PostIt.Domain.Exceptions;

namespace PostIt.Domain.ValueObjects.User;

public class Password : ValueObject
{
    public const int MinLength = 8;
    public const int MaxLength = 32;
    
    public string Value { get; }
    
    private Password() { }

    private Password(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < MinLength)
        {
            throw new DomainException($"Password must be at least {MinLength} characters long.");
        }

        if (password.Length > MaxLength)
        {
            throw new DomainException($"Password must be less than {MaxLength}.");
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