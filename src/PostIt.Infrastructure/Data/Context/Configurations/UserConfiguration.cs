using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects;

namespace PostIt.Infrastructure.Data.Context.Configurations;

/// <summary>
/// Configuration of the user model.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.CreatedAt).IsRequired();
        
        builder.OwnsOne(u => u.Name, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName("Name")
                .HasMaxLength(UserName.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(u => u.Bio, bio =>
        {
            bio.Property(b => b.Value)
                .HasColumnName("Bio")
                .HasMaxLength(UserBio.MaxLength)
                .IsRequired();
        });
        
        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .HasMaxLength(100)
                .IsRequired();
            
            email.HasIndex(e => e.Value)
                .IsUnique();
        });
        
        builder.OwnsOne(u => u.Password, password =>
        {
            password.Property(e => e.Value)
                .HasColumnName("Password")
                .IsRequired();
        });

        builder.Property(u => u.Roles)
            .IsRequired();
        
        builder.ToTable("Users");
    }
}