using System.ComponentModel.DataAnnotations;

namespace PostIt.Domain.Entities;

public abstract class Entity<TKey> where TKey : struct
{
    [Key]
    public virtual TKey Id { get; protected set; }
}