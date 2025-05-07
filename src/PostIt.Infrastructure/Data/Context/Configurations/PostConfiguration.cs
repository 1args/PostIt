using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects;

namespace PostIt.Infrastructure.Data.Context.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.ViewCount).IsRequired();
        builder.Property(p => p.LikesCount).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt).IsRequired(false);
        builder.Property(p => p.Visibility).IsRequired();
        
        builder.OwnsOne(p => p.Title, title =>
        {
            title.Property(t => t.Value)
                .HasColumnName("Title")
                .HasMaxLength(PostTitle.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(p => p.Content, content =>
        {
            content.Property(p => p.Value)
                .HasColumnName("Content")
                .HasMaxLength(PostContent.MaxLength)
                .IsRequired();
        });
        
        builder.HasOne(p => p.Author)
            .WithMany()
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Likes) 
            .WithOne(p => p.Post)
            .HasForeignKey(p => p.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Comments)
            .WithOne(p => p.Post)
            .HasForeignKey(p => p.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.AuthorId);
        builder.HasIndex(p => p.CreatedAt);
        
        builder.ToTable("Posts");
    }
}