using FluentAssertions;
using PostIt.Domain.Entities;
using PostIt.Domain.Enums;
using PostIt.Domain.Exceptions;
using PostIt.Domain.ValueObjects;

namespace PostIt.Domain.Tests.Entities;

public class PostTests
{
    private static (PostTitle title, PostContent content, Guid authorId, DateTime createdAt)
        BuildValidPostData() => 
    (
        PostTitle.Create("Test Title"),
        PostContent.Create("Test Content"),
        Guid.NewGuid(),
        DateTime.UtcNow
    );
    
    private static Post CreatePost() 
    {
        var (title, content, authorId, createdAt) = BuildValidPostData();
        return Post.Create(title, content, authorId, createdAt);
    }

    private static Comment CreateComment(Post post)
    {
        var text = CommentText.Create("Comment text");
        return Comment.Create(text, Guid.NewGuid(), post.Id, DateTime.UtcNow);
    }

    [Fact]
    public void CreatePost_ValidPost_ShouldCreatePostSuccessfully()
    {
        // Arrange
        var (title, content, authorId, createdAt) = BuildValidPostData();
        var precision = TimeSpan.FromSeconds(1);
        
        // Act
        var post = Post.Create(title, content, authorId, createdAt);
        
        // Assert
        post.Title.Should().Be(title);
        post.Content.Should().Be(content);
        post.AuthorId.Should().Be(authorId);
        post.CreatedAt.Should().BeCloseTo(createdAt, precision);
        post.ViewCount.Should().Be(0);
        post.LikesCount.Should().Be(0);
        post.Likes.Should().BeEmpty();
        post.Comments.Should().BeEmpty();
        post.WasUpdated.Should().BeFalse();
    }
    
    [Fact]
    public void CreatePost_FutureDate_ShouldThrowDomainException()
    {
        // Arrange 
        var (title, content, authorId, _) = BuildValidPostData();
        var futureDate = DateTime.UtcNow.AddMinutes(10);
        
        // Act
        Action act = () => Post.Create(title, content, authorId, futureDate);
        
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Creation date cannot be in the future.")
            .Which.ParameterName.Should().Be("createdAt");
    }

    [Fact]
    public void Like_UserCanLikeOnlyOnce()
    {
        // Arrange
        var post = CreatePost();
        var userId = Guid.NewGuid();
        
        // Act
        post.Like(userId);
        
        // Assert
        post.Likes.Should().ContainSingle(l => l.AuthorId == userId);
        post.LikesCount.Should().Be(1);
        
        // Act & Assert
        var secondLike = () => post.Like(userId);
        
        secondLike.Should().Throw<DomainException>()
            .WithMessage($"User with ID {userId} already liked this post.");
    }

    [Fact]
    public void Unlike_UserCanUnlikeOnlyIfLikedBefore()
    {
        // Arrange
        var post = CreatePost();
        var userId = Guid.NewGuid();
        
        // Act
        post.Like(userId);
        post.Unlike(userId);
        
        // Assert
        post.Likes.Should().BeEmpty();
        post.LikesCount.Should().Be(0);
        
        // Act & Assert
        var unlikeAgain = () => post.Unlike(userId);

        unlikeAgain.Should().Throw<DomainException>()
            .WithMessage($"User with ID {userId} not liked this post.");
    }

    [Fact]
    public void View_ShouldIncrementViews()
    {
        // Arrange
        var post = CreatePost();
        
        // Act
        post.View();
        post.View();
        
        // Assert
        post.ViewCount.Should().Be(2);
    }

    [Fact]
    public void AddComment_ShouldAddCommentToList()
    {
        // Arrange
        var post = CreatePost();
        var comment = CreateComment(post);
        
        // Act
        post.AddComment(comment);
        
        // Assert
        post.Comments.Should().ContainSingle().And.Contain(comment);
    }
    
    [Fact]
    public void RemoveComment_ShouldRemoveIfExists()
    {
        // Arrange
        var post = CreatePost();
        var comment = CreateComment(post);
        
        // Act
        post.AddComment(comment);
        post.RemoveComment(comment);
        
        // Assert
        post.Comments.Should().BeEmpty();
    }

    [Fact]
    public void RemoveComment_ShouldThrowIfCommentNotExists()
    {
        // Arrange
        var post = CreatePost();
        var comment = CreateComment(post);
        
        // Act
        var act = () => post.RemoveComment(comment);
        
        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage($"Comment with ID {comment.Id} not found");
    }

    [Fact]
    public void UpdateContent_ShouldUpdateAndSetUpdatedAt()
    {
        // Arrange
        var post = CreatePost();
        var newTitle = PostTitle.Create("New title");
        var newContent = PostContent.Create("New content");
        
        // Act
        post.UpdateContent(newTitle, newContent);
        
        // Assert
        post.Title.Should().Be(newTitle);
        post.Content.Should().Be(newContent);
        post.UpdatedAt.Should().NotBeNull();
        post.WasUpdated.Should().BeTrue();
    }

    [Fact]
    public void UpdateContent_SameContent_ShouldNotUpdate()
    {
        // Arrange
        var (_, _, authorId, createdAt) = BuildValidPostData();
        var title = PostTitle.Create("Same title");
        var content = PostContent.Create("Same content");
        var post = Post.Create(title, content, authorId, createdAt);
        
        // Act
        post.UpdateContent(title, content);
        
        // Assert
        post.UpdatedAt.Should().BeNull();
        post.WasUpdated.Should().BeFalse();
    }

    [Theory]
    [InlineData(Visibility.Public)]
    [InlineData(Visibility.Private)]
    public void SetVisibility_ShouldChangeVisibility(Visibility visibility)
    {
        // Arrange
        var post = CreatePost();
        
        // Act
        post.SetVisibility(visibility);
        
        // Assert
        post.Visibility.Should().Be(visibility);
    }

    [Fact]
    public void IsVisibleToUser_ShouldReturnTrueIfPublic()
    {
        // Arrange
        var post = CreatePost();
        var userId = Guid.NewGuid();
        
        // Act & Assert
        post.IsVisibleToUser(userId).Should().BeTrue();
    }
    
    [Fact]
    public void IsVisibleToUser_ShouldReturnTrueIfPrivateButAuthor()
    {
        // Arrange
        var (title, content, authorId, createdAt) = BuildValidPostData();
        var post = Post.Create(title, content, authorId, createdAt, Visibility.Private);

        // Act & Assert
        post.IsVisibleToUser(authorId).Should().BeTrue();
    }
    
    [Fact]
    public void IsVisibleToUser_ShouldReturnFalseIfPrivateAndNotAuthor()
    {
        // Arrange
        var (title, content, authorId, createdAt) = BuildValidPostData();
        var post = Post.Create(title, content, authorId, createdAt, Visibility.Private);
        var userId = Guid.NewGuid();

        // Act & Assert
        post.IsVisibleToUser(userId).Should().BeFalse();
    }
}