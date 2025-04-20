using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects.Comment;

namespace PostIt.Infrastructure.Data.Context.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.CreatedAt).IsRequired();
        
        builder.OwnsOne(c => c.Text, text =>
        {
            text.Property(t => t.Value)
                .HasColumnName("Text")
                .HasMaxLength(Text.MaxLength)
                .IsRequired();
        });

        builder.HasOne(c => c.Post)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p => p.Author)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Likes)
            .WithOne(cl => cl.Comment)
            .HasForeignKey(cl => cl.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.ToTable("Comments");
    }
}