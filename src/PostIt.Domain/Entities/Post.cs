namespace PostIt.Domain.Entities;

public class Post : Entity<Guid>
{
    private readonly List<Comment> _comments = [];
    
    public string Title { get; set; } = string.Empty;
    
    public string Content { get; set; } = string.Empty;

    public int Views { get; private set; } = 0;
    
    public int Likes { get; private set; } = 0;
    
    public DateTime CreatedAt { get; set; }

    public IReadOnlyList<Comment> Comments => _comments;

    public Guid AuthorId { get; set; }
    
    public Author Author { get; set; } = null!;
}