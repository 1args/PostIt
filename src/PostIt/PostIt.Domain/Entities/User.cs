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
    
    /// <summary>User's email address.</summary>
    public UserEmail Email { get; private set; }
    
    /// <summary>User's password hash.</summary>
    public UserPassword Password { get; private set; }
    
    private readonly List<Role> _roles = [];
    
    /// <summary>Roles of the user.</summary>
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    private readonly List<User> _followers = [];
    
    /// <summary>Users who follow this user.</summary>
    public IReadOnlyCollection<User> Followers => _followers.AsReadOnly();
    
    private readonly List<User> _followings = [];
    
    /// <summary>Users this user is following.</summary>
    public IReadOnlyCollection<User> Followings => _followings.AsReadOnly();

    /// <summary>Indicates whether the user's email has been verified.</summary>
    public bool IsEmailConfirmed  { get; private set; }

    /// <summary>Path to the user's avatar image. Can be null if not set.</summary>
    public string? Avatar { get; private set; }

    /// <summary>Date when the user was created.</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>EF navigation property.</summary>
    public IReadOnlyCollection<Post> Posts { get; private set; } = null!;
    
    /// <summary>EF navigation property.</summary>
    public IReadOnlyCollection<Comment> Comments { get; private set; } = null!;

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
    
    /// <summary>
    /// Adds a follower to this user.
    /// </summary>
    /// <param name="follower">The user who wants to follow.</param>
    /// <exception cref="DomainException">Thrown if the follower is already following or is the same user.</exception>
    public void AddFollower(User follower)
    {
        if (follower.Id == Id)
        {
            throw new DomainException("A user cannot follow themselves.");
        }
        if (_followers.Contains(follower))
        {
            throw new DomainException("This user is already a follower.");
        }
        _followers.Add(follower);
    }
    
    /// <summary>
    /// Removes a follower from this user.
    /// </summary>
    /// <param name="follower">The user to unfollow.</param>
    /// <exception cref="DomainException">Thrown אם the follower is not following this user.</exception>
    public void RemoveFollower(User follower)
    {
        if (!_followers.Remove(follower))
        {
            throw new DomainException("This user is not a follower.");
        }
    }
    
    /// <summary>
    /// Adds a user to this user's followings.
    /// </summary>
    /// <param name="following">The user to follow.</param>
    /// <exception cref="DomainException">Thrown if already following or is the same user.</exception>
    public void AddFollowing(User following)
    {
        if (following.Id == Id)
        {
            throw new DomainException("A user cannot follow themselves.");
        }
        if (_followings.Contains(following))
        {
            throw new DomainException("This user is already being followed.");
        }
        _followings.Add(following);
    }
    
    /// <summary>
    /// Removes a user from this user's followings.
    /// </summary>
    /// <param name="following">The user to unfollow.</param>
    /// <exception cref="DomainException">Thrown if the user is not being followed.</exception>
    public void RemoveFollowing(User following)
    {
        if (!_followings.Remove(following))
        {
            throw new DomainException("This user is not being followed.");
        }
    }
}