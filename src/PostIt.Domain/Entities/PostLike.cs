namespace PostIt.Domain.Entities;

/// <summary>
/// Represents a like on a post.
/// </summary>
public class PostLike
{
    /// <summary>ID of the liked post.</summary>
    public Guid PostId { get; private set; }
    
    /// <summary>ID of the user who liked the post.</summary>
    public Guid AuthorId { get; private set; }
    
    /// <summary>Post navigation property.</summary>
    public Post Post { get; private set; } = null!;

    /// <summary>Author navigation property.</summary>
    public User Author { get; private set; } = null!;

    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private PostLike(Guid postId, Guid authorId)
    {
        PostId = postId;
        AuthorId = authorId;
    }
    
    /// <summary>
    /// Factory method to create a new post like.
    /// </summary>
    /// <param name="postId">The ID of the post.</param>
    /// <param name="authorId">The ID of the author.</param>
    /// <returns>A new instance of <see cref="PostLike"/>.</returns>
    public static PostLike Create(Guid postId, Guid authorId) => new(postId, authorId);
}