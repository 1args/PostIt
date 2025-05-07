using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects;
using PostIt.Domain.ValueObjects.User;

namespace PostIt.Infrastructure.Data.Context.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
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

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .IsRequired();
        
        builder.ToTable("Users");
    }
}