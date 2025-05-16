using PostIt.Domain.Enums;
using PostIt.Domain.Exceptions;
using PostIt.Domain.Primitives;
using PostIt.Domain.ValueObjects;

namespace PostIt.Domain.Entities;

/// <summary>
/// Represents a user entity.
/// </summary>
public class User : Entity<Guid>, IAuditableEntity
{
    /// <summary>User's display name.</summary>
    public UserName Name { get; private set; }

    /// <summary>User's biography.</summary>
    public UserBio Bio { get; private set; }

    /// <summary>Number of posts created by the user.</summary>
    public int PostsCount { get; private set; }
    
    /// <summary>Number of comments created by the user.</summary>
    public int CommentsCount { get; private set; }
    
    /// <summary>User's email address.</summary>
    public UserEmail Email { get; private set; }
    
    /// <summary>User's password hash.</summary>
    public UserPassword Password { get; private set; }
    
    private readonly List<Role> _roles = [];
    
    /// <summary>Roles of the user.</summary>
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    /// <summary>Indicates whether the user's email has been verified.</summary>
    public bool IsEmailConfirmed  { get; private set; }

    /// <summary>Path to the user's avatar image. Can be null if not set.</summary>
    public string? Avatar { get; private set; }

    /// <summary>Date when the user was created.</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Constructor for EF Core.
    /// </summary>
    private User() { }

    /// <summary>
    /// Private constructor used by the factory Create method.
    /// </summary>
    private User(
        UserName name,
        UserBio bio,
        UserEmail email, 
        UserPassword password, 
        DateTime createdAt)
    {
        Name = name;
        Bio = bio.IsEmpty() ? UserBio.Create("Empty") : bio;
        Email = email;
        Password = password;
        CreatedAt = createdAt;
    }
    
    /// <summary>
    /// Factory method to create a new user instance.
    /// </summary>
    /// <param name="name">The display name.</param>
    /// <param name="bio">The biography.</param>
    /// <param name="email">The email address.</param>
    /// <param name="password">The hashed password.</param>
    /// <param name="createdAt">The creation timestamp.</param>
    /// <returns>A new instance of the <see cref="User"/> class.</returns>
    public static User Create(
        UserName name,
        UserBio bio, 
        UserEmail email, 
        UserPassword password,
        IReadOnlyCollection<Role> roles,
        DateTime createdAt) =>
        new(name, bio, email, password, createdAt);

    /// <summary>
    /// Marks the user's email as verified.
    /// </summary>
    /// <exception cref="DomainException">
    /// Thrown if the user has already confirmed the email.
    /// </exception>
    public void ConfirmEmail()
    {
        if (IsEmailConfirmed)
        {
            throw new DomainException("Email already confirmed.");
        }
        IsEmailConfirmed = true;
    }

    /// <summary>
    /// Updates the user's biography.
    /// </summary>
    /// <param name="bio">The new biography.</param>
    public void UpdateBio(UserBio bio)
    {
        if (Bio.Equals(bio))
        {
            return;
        }
        Bio = bio;
    }
    
    public void IncrementPostsCount() => PostsCount++;
    
    public void DecrementPostsCount()
    {
        if (PostsCount <= 0) throw new DomainException("Posts count cannot be negative.");
        PostsCount--;
    }
    
    public void IncrementCommentsCount() => CommentsCount++;
    
    public void DecrementCommentsCount()
    {
        if (CommentsCount <= 0) throw new DomainException("Comment count cannot be negative.");
        CommentsCount--;
    }

    /// <summary>
    /// Updates the user's avatar.
    /// </summary>
    /// <param name="avatar">The new avatar path.</param>
    /// <exception cref="DomainException">
    /// Thrown when the provided value is null, empty, or consists only of white-space characters.
    /// </exception>
    public void UpdateAvatar(string avatar)
    {
        if (string.IsNullOrWhiteSpace(avatar))
        {
            throw new DomainException("Avatar cannot be empty.");
        }
        Avatar = avatar;
    }

    /// <summary>
    /// Adds a role to the user's roles.
    /// </summary>
    /// <param name="role">The role to add.</param>
    /// <exception cref="DomainException">Thrown if the role is already assigned to the user.</exception>
    public void AddRole(Role role)
    {
        if (_roles.Contains(role))
        {
            throw new DomainException($"Role '{role.Name}' is already assigned to the user.");
        }
        
        _roles.Add(role);
    }
    
    /// <summary>
    /// Removes a role from the user's roles.
    /// </summary>
    /// <param name="role">The role to remove.</param>
    /// <exception cref="DomainException">Thrown if the role is not assigned to the user.</exception>
    public void RemoveRole(Role role)
    {
        if (!_roles.Remove(role))
        {
            throw new DomainException($"Role '{role.Name}' is not assigned to the user.");
        }
    }
}