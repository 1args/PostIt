using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostIt.Domain.Entities;
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
                .HasMaxLength(Name.MaxLength)
                .IsRequired();
        });

        builder.OwnsOne(u => u.Bio, bio =>
        {
            bio.Property(b => b.Value)
                .HasColumnName("Bio")
                .HasMaxLength(Bio.MaxLength)
                .IsRequired();
        });
        
        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.OwnsOne(u => u.Password, password =>
        {
            password.Property(p => p.Value)
                .HasColumnName("Password")
                .HasMaxLength(Password.MaxLength)
                .IsRequired();
        });

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .IsRequired();
        
        builder.ToTable("Users");
    }
}