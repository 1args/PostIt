using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects.Author;

namespace PostIt.Infrastructure.Context.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();

        builder.OwnsOne(a => a.Name, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName("Name")
                .HasMaxLength(Name.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(a => a.Bio, bio =>
        {
            bio.Property(b => b.Value)
                .HasColumnName("Bio")
                .HasMaxLength(Bio.MaxLength)
                .IsRequired();
        });

        builder.HasOne(a => a.User)
            .WithOne(u => u.Author)
            .HasForeignKey<Author>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.ToTable("Authors");
    }
}