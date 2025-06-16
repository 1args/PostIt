using FluentAssertions;
using PostIt.Domain.Entities;
using PostIt.Domain.Exceptions;
using PostIt.Domain.ValueObjects;

namespace Tests.Domain;

public class UserTests
{
    private static (UserName name, UserBio bio, UserEmail email, UserPassword password, DateTime createdAt)
        BuildValidUserData() =>
    (
        UserName.Create("John Doe"),
        UserBio.Create("Bio"),
        UserEmail.Create("john.doe@gmail.com"),
        UserPassword.Create("password123"),
        DateTime.UtcNow
    );

    [Fact]
    public void CreateUser_ValidUser_ShouldCreateUserSuccessfully()
    {
        // Arrange
        var (name, bio, email, password, createdAt) = BuildValidUserData();
        var precision = TimeSpan.FromSeconds(1);

        // Act
        var user = User.Create(name, bio, email, password, createdAt);

        // Assert
        user.Should().BeEquivalentTo(new
        {
            Name = name,
            Bio = bio,
            Email = email,
        });
        user.CreatedAt.Should().BeCloseTo(createdAt, precision);
        user.Posts.Count.Should().Be(0);
        user.Comments.Count.Should().Be(0);
    }

    [Fact]
    public void CreateUser_FutureDate_ShouldThrowDomainException()
    {
        // Arrange 
        var (name, bio, email, password, _) = BuildValidUserData();
        var futureDate = DateTime.UtcNow.AddMinutes(10);

        // Actions
        Action act = () => User.Create(name, bio, email, password, futureDate);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Creation date cannot be in the future.");
    }

    [Theory]
    [InlineData("", "Email cannot be empty.")]
    [InlineData("   ", "Email cannot be empty.")]
    [InlineData("invalid-email", "Invalid email format.")]
    [InlineData("john.doe@", "Invalid email format.")]
    [InlineData("john.doe.com", "Invalid email format.")]
    [InlineData("@gmail.com", "Invalid email format.")]
    public void CreateUser_InvalidEmail_ShouldThrowDomainException(string invalidEmail, string expectedMessage)
    {
        // Act
        Action act = () => UserEmail.Create(invalidEmail);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage(expectedMessage);
    }
}