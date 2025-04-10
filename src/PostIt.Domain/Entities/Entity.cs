using System.ComponentModel.DataAnnotations;

namespace PostIt.Domain.Entities;

public abstract class Entity<TKey> where TKey : struct
{
    [Key]
    protected virtual TKey Id { get; set; }
}