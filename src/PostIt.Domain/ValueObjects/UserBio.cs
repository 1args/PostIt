using PostIt.Domain.Exceptions;
using PostIt.Domain.Primitives;

namespace PostIt.Domain.ValueObjects.User;

public class UserBio : ValueObject
{
    public const int MaxLength = 150;
    
    public string Value { get; }

    private UserBio() { }
    
    private UserBio(string bio)
    {
        if(string.IsNullOrWhiteSpace(bio))
        {
            Value = "Empty";
            return;
        }

        if (bio.Length > MaxLength)
        {
            throw new DomainException($"User bio must be no longer than {MaxLength} characters long.");
        }
        
        Value = bio;
    }

    public static UserBio Create(string bio) => new(bio);
    
    public bool IsEmpty() => string.IsNullOrWhiteSpace(Value);

    public override string ToString() => Value;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}