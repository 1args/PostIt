using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects;

namespace PostIt.Infrastructure.Data.Context.Configurations;

/// <summary>
/// Configuration of the comment model.
/// </summary>
public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Text)
            .HasConversion(text => text.Value, v => CommentText.Create(v))
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();
        
        builder.HasIndex(c => c.CreatedAt);

        builder.HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p => p.Author)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Likes)
            .WithOne(cl => cl.Comment)
            .HasForeignKey(cl => cl.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Navigation(c => c.Likes)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        
        builder.HasIndex(c => c.PostId);
        
        builder.HasIndex(c => c.AuthorId);
        
        builder.ToTable("Comments");
    }
}