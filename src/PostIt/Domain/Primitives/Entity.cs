using System.ComponentModel.DataAnnotations;

namespace PostIt.Domain.Primitives;

/// <summary>
/// Entity base class with primary key.
/// </summary>
/// <typeparam name="TKey">Structure that defines the type of identifier.</typeparam>
public abstract class Entity<TKey> where TKey : struct
{
    /// <summary>
    /// Unique entity identifier.
    /// </summary>
    [Key]
    public virtual TKey Id { get; set; }
}