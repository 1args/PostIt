using FluentAssertions;
using PostIt.Domain.Entities;
using PostIt.Domain.Enums;
using PostIt.Domain.Exceptions;
using PostIt.Domain.ValueObjects.User;

namespace PostIt.Domain.Tests.Entities;

public class UserTests
{
    private static (Name name, Bio bio, Email email, Password password, Role role) 
        BuildValidUserData() =>
    (
        Name.Create("John Doe"),
        Bio.Create("Bio"),
        Email.Create("john.doe@gmail.com"),
        Password.Create("password123"),
        Role.User
    );
    
    [Fact]
    public void CreateUser_ValidUser_ShouldCreateUserSuccessfully()
    {
        // Arrange
        var (name, bio, email, password, role) = BuildValidUserData();
        
        // Act
        var before = DateTime.UtcNow;
        var user = User.Create(name, bio, email, password, role, before);
        var after = DateTime.UtcNow;
        
        // Assert
        user.Should().BeEquivalentTo(new
        {
            Name = name,
            Bio = bio,
            Email = email,
            Password = password,
            Role = role
        });
        user.CreatedAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
        user.Posts.Should().BeEmpty();
        user.Comments.Should().BeEmpty();
    }

    [Fact]
    public void CreateUser_FutureDate_ShouldThrowDomainException()
    {
        // Arrange 
        var (name, bio, email, password, role) = BuildValidUserData();
        var futureDate = DateTime.UtcNow.AddMinutes(10);
        
        // Action
        Action act = () => User.Create(name, bio, email, password, role, futureDate);
        
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Creation date cannot be in the future.")
            .Which.ParameterName.Should().Be("createdAt");
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
        Action act = () => Email.Create(invalidEmail);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage(expectedMessage);
    }

    [Fact]
    public void CreateUser_ShortPassword_ShouldThrowDomainException()
    {
        // Act
        Action act = () => Password.Create("123");
        
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage($"Password must be at least {Password.MinLength} characters long.");
    }

    [Fact]
    public void CreateUser_TooLongPassword_ShouldThrowDomainException()
    {
        // Act
        Action act = () => Password.Create(new string('a', Password.MaxLength + 1));
        
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage($"Password must be less than {Password.MaxLength}.");
    }

    [Fact]
    public void CreateUser_TooLongBio_ShouldThrowDomainException()
    {
        // Act
        Action act = () => Bio.Create(new string('a', Bio.MaxLength + 1));
        
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage($"User bio must be no longer than {Bio.MaxLength} characters.");
    }
    
    [Fact]
    public void CreateUser_ShortName_ShouldThrowDomainException()
    {
        // Act
        Action act = () => Name.Create("Jo");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage($"User name must be at least {Name.MinLength} characters long.");
    }
    
    [Fact]
    public void CreateUser_TooLongName_ShouldThrowDomainException()
    {
        // Act
        Action act = () => Name.Create(new string('a', Name.MaxLength + 1));

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage($"User name must be no longer than {Name.MaxLength} characters.");
    }
}