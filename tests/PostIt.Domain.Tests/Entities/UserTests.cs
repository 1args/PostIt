using FluentAssertions;
using PostIt.Domain.Entities;
using PostIt.Domain.Enums;
using PostIt.Domain.ValueObjects.User;

namespace PostIt.Domain.Tests.Entities;

public class UserTests
{
    private static (Name name, Bio bio, Email email, Password password, Role role, DateTime createdAt) 
        BuildValidUserData() =>
    (
        Name.Create("John Doe"),
        Bio.Create("Bio"),
        Email.Create("john.doe@gmail.com"),
        Password.Create("password123"),
        Role.User,
        DateTime.Now
    );
    
    [Fact]
    public void CreateUser_ValidUser_ShouldCreateUserSuccessfully()
    {
        // Arrange
        var (name, bio, email, password, role, createdAt) = BuildValidUserData();
        var precision = TimeSpan.FromSeconds(1);
        
        // Act
        var user = User.Create(name, bio, email, password, role, createdAt);
        
        // Assert
        user.Should().BeEquivalentTo(new
        {
            Name = name,
            Bio = bio,
            Email = email,
            Password = password,
            Role = role
        });
        user.CreatedAt.Should().BeCloseTo(createdAt, precision: precision);
        user.Posts.Should().BeEmpty();
        user.Comments.Should().BeEmpty();
    }
}