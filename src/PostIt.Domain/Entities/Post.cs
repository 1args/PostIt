using PostIt.Domain.Enums;
using PostIt.Domain.Exceptions;
using PostIt.Domain.Primitives;
using PostIt.Domain.ValueObjects;

namespace PostIt.Domain.Entities;

/// <summary>
/// Represents a post entity.
/// </summary>
public class Post : Entity<Guid>, IAuditableEntity, IAuthoredEntity
{
    /// <summary>Title of the post.</summary>
    public PostTitle Title { get; private set; } 
    
    /// <summary>Content of the post.</summary>
    public PostContent Content { get; private set; }

    /// <summary>Total number of views of the post.</summary>
    public int ViewCount { get; private set; }
    
    /// <summary>Number of likes on the post.</summary>
    public int LikesCount { get; private set; }
    
    /// <summary>Date when the post was created.</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>Date when the comment was last updated, if any.</summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>Indicates whether the post has been updated.</summary>
    public bool WasUpdated => UpdatedAt.HasValue;

    /// <summary>Visibility status of the post.</summary>
    public Visibility Visibility { get; private set; }

    private readonly List<PostLike> _likes = [];
    
    /// <summary>Read-only list of likes on the post.</summary>
    public IReadOnlyList<PostLike> Likes => _likes.AsReadOnly();
    
    private readonly List<Comment> _comments = [];
    
    /// <summary>Read-only list of comments on the post.</summary>
    public IReadOnlyList<Comment> Comments => _comments.AsReadOnly();

    /// <summary>ID of the post's author.</summary>
    public Guid AuthorId { get; private set; }
    
    /// <summary>Author navigation property.</summary>
    public User Author { get; private set; } = null!;

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private Post() { }
    
    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private Post(
        PostTitle title, 
        PostContent content,
        Guid authorId, 
        DateTime createdAt,
        Visibility visibility = Visibility.Public)
    {
        ValidateVisibility(visibility);

        Title = title;
        Content = content;
        AuthorId = authorId;
        CreatedAt = createdAt;
        Visibility = visibility;
    }
    
    /// <summary>
    /// Factory method to create a new post.
    /// </summary>
    /// <param name="title">The title of the post.</param>
    /// <param name="content">The content of the post.</param>
    /// <param name="authorId">The ID of the post's author.</param>
    /// <param name="createdAt">The creation timestamp.</param>
    /// <param name="visibility">The visibility of the post (default is public).</param>
    /// <returns>A new instance of the <see cref="Post"/> class.</returns>
    public static Post Create(
        PostTitle title, 
        PostContent content,
        Guid authorId,
        DateTime createdAt,
        Visibility visibility = Visibility.Public) =>
            new(title, content, authorId, createdAt, visibility);

    /// <summary>
    /// Adds a like to the post from a user.
    /// </summary>
    /// <param name="userId">The ID of the user liking the post.</param>
    /// <exception cref="DomainException">
    /// Thrown if the user already liked the post.
    /// </exception>
    public void Like(Guid userId)
    {
        if(_likes.Any(l => l.AuthorId == userId))
        {
            throw new DomainException($"User with ID {userId} already liked this post.");
        }
        _likes.Add(PostLike.Create(Id, userId));
        LikesCount++;
    }

    /// <summary>
    /// Removes a like from the post by a user.
    /// </summary>
    /// <param name="userId">The ID of the user unliking the post.</param>
    /// <exception cref="DomainException">
    /// Thrown if the user has not liked the post.
    /// </exception>
    public void Unlike(Guid userId)
    {
        var like = _likes.FirstOrDefault(l => l.AuthorId == userId);
        
        if (like is null)
        {
            throw new DomainException($"User with ID {userId} not liked this post.");
        }
        _likes.Remove(like);
        LikesCount--;
    }
    
    /// <summary>
    /// Increments the view count for the post.
    /// </summary>
    public void View() => ViewCount++;

    /// <summary>
    /// Adds a comment to the post.
    /// </summary>
    /// <param name="comment">The comment to add.</param>
    public void AddComment(Comment comment)
    {
        ArgumentNullException.ThrowIfNull(comment);
        _comments.Add(comment);
    }

    /// <summary>
    /// Removes a comment from the post.
    /// </summary>
    /// <param name="comment">The comment to remove.</param>
    /// <exception cref="DomainException">
    /// Thrown if the comment is not found in the post.
    /// </exception>
    public void RemoveComment(Comment comment)
    {
        if (!_comments.Contains(comment))
        {
            throw new DomainException($"Comment with ID {comment.Id} not found");
        }
        _comments.Remove(comment);
    }
    
    /// <summary>
    /// Updates the content and title of the post.
    /// </summary>
    public void UpdateContent(PostTitle newPostTitle, PostContent newContent)
    {
        ArgumentNullException.ThrowIfNull(newPostTitle);
        ArgumentNullException.ThrowIfNull(newContent);

        if (Title.Equals(newPostTitle) && Content.Equals(newContent))
        {
            return;
        }
        
        Title = newPostTitle;
        Content = newContent;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Sets the visibility of the post.
    /// </summary>
    /// <param name="visibility">The new visibility status.</param>
    public void SetVisibility(Visibility visibility)
    {
        ValidateVisibility(visibility);
        Visibility = visibility;
    }

    /// <summary>
    /// Determines if the post is visible to a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns><c>true</c> if the post is visible; otherwise, <c>false</c>.</returns>
    public bool IsVisibleToUser(Guid userId) =>
        Visibility == Visibility.Public || AuthorId == userId;
    
    private static void ValidateVisibility(Visibility visibility)
    {
        if (visibility != Visibility.Public && visibility != Visibility.Private)
        {
            throw new DomainException("Invalid visibility value.");
        }
    }
}