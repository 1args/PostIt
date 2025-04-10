using System.Text.RegularExpressions;

namespace PostIt.Domain.ValueObjects;

public class Email
{
    public string Value { get; }

    private Email(string email) 
    {
        if (!IsEmailValid(email))
        {
            throw new ArgumentException("Invalid email format.", nameof(email));
        }
        
        Value = email;
    }

    public static Email Create(string email) => new(email);

    public static bool IsEmailValid(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be empty.", nameof(email));
        }

        var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }
    
    public override string ToString() => Value;
}