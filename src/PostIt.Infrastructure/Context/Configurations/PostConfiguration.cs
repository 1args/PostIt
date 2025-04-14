using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects.Post;

namespace PostIt.Infrastructure.Context.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.OwnsOne(p => p.Title, title =>
        {
            title.Property(t => t.Value)
                .HasColumnName("Title")
                .HasMaxLength(Title.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(p => p.Content, content =>
        {
            content.Property(p => p.Value)
                .HasColumnName("Content")
                .HasMaxLength(Content.MaxLength)
                .IsRequired();
        });
    }
}