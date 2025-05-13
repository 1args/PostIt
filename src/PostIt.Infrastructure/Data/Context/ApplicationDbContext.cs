using Microsoft.EntityFrameworkCore;
using PostIt.Domain.Entities;

namespace PostIt.Infrastructure.Data.Context;

/// <summary>
/// Basic database context
/// </summary>
public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
    
    public DbSet<Post> Posts { get; set; }
    
    public DbSet<PostLike> PostLikes { get; set; }
    
    public DbSet<Comment> Comments { get; set; }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CustomModelBuilder.OnModelCreating(modelBuilder);
    }
}