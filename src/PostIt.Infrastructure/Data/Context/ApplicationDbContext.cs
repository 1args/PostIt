using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PostIt.Domain.Entities;
using PostIt.Infrastructure.Data.Context.Configurations;
using PostIt.Infrastructure.Options;

namespace PostIt.Infrastructure.Data.Context;

/// <summary>
/// Basic database context
/// </summary>
public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IOptions<AuthorizationOptions> authorizationOptions) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Role> Roles { get; set; }

    public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
    
    public DbSet<Post> Posts { get; set; }
    
    public DbSet<PostLike> PostLikes { get; set; }
    
    public DbSet<Comment> Comments { get; set; }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CustomModelBuilder.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authorizationOptions.Value));
    }
}