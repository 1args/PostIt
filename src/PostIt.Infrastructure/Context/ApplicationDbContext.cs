using Microsoft.EntityFrameworkCore;
using PostIt.Domain.Entities;
using PostIt.Infrastructure.Context.Configurations;

namespace PostIt.Infrastructure.Context;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Post> Posts { get; set; }
    
    public DbSet<PostLike> PostLikes { get; set; }
    
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
    }
}