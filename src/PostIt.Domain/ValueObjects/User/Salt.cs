using System.Security.Cryptography;
using PostIt.Domain.Exceptions;

namespace PostIt.Domain.ValueObjects.User;

public class Salt : ValueObject
{
    public const int Minlength = 16;
    public const int Maxlength = 64;
    public string Value { get; }

    private Salt(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new DomainException("Salt cannot be empty.");
        }

        if (value.Length is < Minlength or > Maxlength) 
        {
            throw new DomainException($"Salt length must be between {Minlength} and {Maxlength} characters.");
        }

        Value = value;
    }

    public static Salt Create(string salt) => new Salt(salt);
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}