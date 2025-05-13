using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostIt.Domain.Entities;

namespace PostIt.Infrastructure.Data.Context.Configurations;

/// <summary>
/// Configuration of the comment like model.
/// </summary>
public class CommentLikeConfiguration : IEntityTypeConfiguration<CommentLike>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<CommentLike> builder)
    {
        builder.HasKey(cl => new { cl.CommentId, cl.AuthorId });
        
        builder.HasOne(cl => cl.Comment)
            .WithMany(c => c.Likes) 
            .HasForeignKey(cl => cl.CommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cl => cl.Author)
            .WithMany()
            .HasForeignKey(cl => cl.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.ToTable("CommentLikes");
    }
}