using PostIt.Domain.Exceptions;

namespace PostIt.Domain.ValueObjects.User;

public class Name : ValueObject
{
    public const int MinLength = 3;
    public const int MaxLength = 30;
    
    public string Value { get; }
    
    private Name() { }
    
    private Name(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("User name cannot be empty.");
        }

        Value = name.Length switch
        {
            < MinLength => throw new DomainException(
                $"User name must be at least {MinLength} characters long."),
            > MaxLength => throw new DomainException(
                $"User name must be no longer than {MaxLength} characters."),
            _ => name
        };
    }
    
    public static Name Create(string name) => new(name);

    public override string ToString() => Value;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}