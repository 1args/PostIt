namespace PostIt.Domain.Entities;

public class Comment : Entity<Guid>
{
    public string Text { get; set; }

    public int Likes { get; set; } = 0;
    
    public DateTime CreatedAt { get; set; }
    
    public Guid AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    public Guid PostId { get; set; }
    public Post Post { get; set; } = null!;
}