using System.Text.RegularExpressions;
using PostIt.Domain.Exceptions;

namespace PostIt.Domain.ValueObjects.User;

public class Email : ValueObject
{
    public string Value { get; }

    private Email(string email) 
    {
        if (!IsEmailValid(email))
        {
            throw new DomainException("Invalid email format.");
        }
        
        Value = email;
    }

    public static Email Create(string email) => new(email);

    public static bool IsEmailValid(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("Email cannot be empty.");
        }

        var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }
    
    public override string ToString() => Value;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}