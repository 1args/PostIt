using Microsoft.EntityFrameworkCore;
using PostIt.Domain.Entities;

namespace PostIt.Infrastructure.Context;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Author> Authors { get; set; }
    
    public DbSet<Post> Posts { get; set; }
    
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}