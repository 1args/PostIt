using PostIt.Domain.Exceptions;
using PostIt.Domain.Primitives;

namespace PostIt.Domain.ValueObjects;

public class UserPassword : ValueObject
{
    public const int MinLength = 8;
    public const int MaxLength = 100;
    
    public string Value { get; private set; }
    
    private UserPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new DomainException("Password cannot be empty.");
        }
        
        Value = password;
    }
    
    public static UserPassword Create(string password) => new(password);

    public static bool IsPasswordStrong(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }
        if (password.Length is < MinLength or > MaxLength)
        {
            return false;
        }
        
        var hasUpper = password.Any(char.IsUpper);
        var hasLower = password.Any(char.IsLower);
        var hasDigit = password.Any(char.IsDigit);
        var hasSymbol = password.Any(ch => char.IsSymbol(ch) || char.IsPunctuation(ch));
        
        return hasUpper && hasLower && hasDigit && hasSymbol;
    }
    
    public override string ToString() => Value;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}