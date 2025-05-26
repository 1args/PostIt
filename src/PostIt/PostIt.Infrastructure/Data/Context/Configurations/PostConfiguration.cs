using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects;

namespace PostIt.Infrastructure.Data.Context.Configurations;

/// <summary>
/// Configuration of the post model.
/// </summary>
public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .HasConversion(title => title.Value, v => PostTitle.Create(v))
            .HasMaxLength(PostTitle.MaxLength)
            .IsRequired();

        builder.Property(p => p.Content)
            .HasConversion(content => content.Value, v => PostContent.Create(v))
            .HasMaxLength(PostContent.MaxLength)
            .IsRequired();

        builder.Property(p => p.ViewCount)
            .IsRequired();
        
        builder.Property(p => p.CreatedAt)
            .IsRequired();
        
        builder.HasIndex(p => p.CreatedAt);
        
        builder.Property(p => p.UpdatedAt)
            .IsRequired(false);
        
        builder.Property(p => p.Visibility)
            .HasConversion<int>()
            .IsRequired();
        
        builder.HasOne(p => p.Author)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(p => p.Likes) 
            .WithOne(p => p.Post)
            .HasForeignKey(p => p.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Navigation(p => p.Likes)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(p => p.Comments)
            .WithOne(p => p.Post)
            .HasForeignKey(p => p.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Navigation(p => p.Comments)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(p => p.AuthorId);
        
        builder.ToTable("Posts");
    }
}