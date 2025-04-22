using FluentAssertions;
using PostIt.Domain.Entities;
using PostIt.Domain.Exceptions;
using PostIt.Domain.ValueObjects.Comment;

namespace PostIt.Domain.Tests.Entities;

public class CommentTests
{
    private static (Text text, Guid authorId, Guid postId, DateTime createdAt) 
        BuildValidCommentData() => 
    (
        Text.Create("This is a test comment."),
        Guid.NewGuid(),
        Guid.NewGuid(),
        DateTime.UtcNow
    );
    
    private static Comment CreateComment()
    {
        var (text, authorId, postId, createdAt) = BuildValidCommentData();
        return Comment.Create(text, authorId, postId, createdAt);
    }

    [Fact]
    public void CreateComment_ValidComment_ShouldCreateCommentSuccessfully()
    {
        // Arrange
        var (text, authorId, postId, createdAt) = BuildValidCommentData();
        var precision = TimeSpan.FromSeconds(1);
        
        // Act
        var comment = Comment.Create(text, authorId, postId, createdAt);
        
        // Assert
        comment.Text.Should().Be(text);
        comment.AuthorId.Should().Be(authorId);
        comment.PostId.Should().Be(postId);
        comment.CreatedAt.Should().BeCloseTo(createdAt, precision);
        comment.Likes.Should().BeEmpty();
    }

    [Fact]
    public void CreateComment_FutureDate_ShouldThrowDomainException()
    {
        // Arrange 
        var (text, authorId, postId, _) = BuildValidCommentData();
        var futureDate = DateTime.UtcNow.AddMinutes(10);
        
        // Act
        Action act = () => Comment.Create(text, authorId, postId, futureDate);
        
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Creation date cannot be in the future.");
    }

    [Fact]
    public void Like_UserCanLikeOnlyOnce()
    {
        // Arrange 
        var comment = CreateComment();
        var userId = Guid.NewGuid();
        
        // Act
        comment.Like(userId);
        
        // Assert
        comment.Likes.Should().ContainSingle(l => l.AuthorId == userId);
        
        // Act & Assert
        var secondLike = () => comment.Like(userId);
        
        secondLike.Should().Throw<DomainException>()
            .WithMessage($"User with ID {userId} already liked this comment.");
    }

    [Fact]
    public void Unlike_UserCanUnlikeOnlyIfLikedBefore()
    {
        // Arrange
        var comment = CreateComment();
        var userId = Guid.NewGuid();
        
        // Act
        comment.Like(userId);
        comment.Unlike(userId);
        
        // Assert
        comment.Likes.Should().BeEmpty();
        
        // Act && Assert
        var unlikedAgain = () => comment.Unlike(userId);
        
        unlikedAgain.Should().Throw<DomainException>()
            .WithMessage($"User with ID {userId} not liked this comment.");
    }
}