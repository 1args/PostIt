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

        builder.Property(u => u.Name)
            .HasConversion(name => name.Value, v => UserName.Create(v))
            .HasMaxLength(UserName.MaxLength)
            .IsRequired();

        builder.Property(u => u.Bio)
            .HasConversion(bio => bio.Value, v => UserBio.Create(v))
            .HasMaxLength(UserBio.MaxLength)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasConversion(email => email.Value, v => UserEmail.Create(v))
            .HasMaxLength(UserEmail.MaxLength)
            .IsRequired();
        
        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Password)
            .HasConversion(password => password.Value, v => UserPassword.Create(v))
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .IsRequired();
        
        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<UserRole>();

        builder.Navigation(u => u.Roles)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(u => u.Followers)
            .WithMany(u => u.Followings)
            .UsingEntity(j => j.ToTable("UserFollowers"));
        
        builder.Navigation(u => u.Followers)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        
        builder.Navigation(u => u.Followings)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(u => u.Posts)
            .WithOne(p => p.Author)
            .HasForeignKey(p => p.AuthorId);
        
        builder.HasMany(u => u.Comments)
            .WithOne(c => c.Author)
            .HasForeignKey(c => c.AuthorId);
        
        builder.ToTable("Users");
    }
}