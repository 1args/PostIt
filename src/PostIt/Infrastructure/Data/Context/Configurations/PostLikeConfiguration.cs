using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostIt.Domain.Entities;

namespace PostIt.Infrastructure.Data.Context.Configurations;

/// <summary>
/// Configuration of the post like model.
/// </summary>
public class PostLikeConfiguration : IEntityTypeConfiguration<PostLike>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<PostLike> builder)
    {
        builder.HasKey(pl => new { pl.PostId, pl.AuthorId });
        
        builder.HasOne(pl => pl.Post)
            .WithMany(p => p.Likes)
            .HasForeignKey(pl => pl.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pl => pl.Author)
            .WithMany()
            .HasForeignKey(pl => pl.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.ToTable("PostLikes");
    }
}