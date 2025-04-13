namespace PostIt.Domain.ValueObjects;

public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }
        
        var other = (ValueObject)obj;
        
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(1, HashCode.Combine);
    }
    
    public static bool operator ==(ValueObject left, ValueObject right) =>
        Equals(left, right);
    
    public static bool operator !=(ValueObject left, ValueObject right) =>
        !Equals(left, right);
}