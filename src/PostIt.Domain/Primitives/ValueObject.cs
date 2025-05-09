namespace PostIt.Domain.Primitives;

/// <summary>
/// Base class for value objects, which are compared by value.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Returns the components that are used to determine equality.
    /// </summary>
    protected abstract IEnumerable<object> GetEqualityComponents();
    
    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }
        
        var other = (ValueObject)obj;
        
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }
    
    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(1, HashCode.Combine);
    }

    /// <summary>
    /// Equality operator
    /// </summary>
    public static bool operator ==(ValueObject left, ValueObject right) =>
        Equals(left, right);
    
    /// <summary>
    /// Inequality operator
    /// </summary>
    public static bool operator !=(ValueObject left, ValueObject right) =>
        !Equals(left, right);
}