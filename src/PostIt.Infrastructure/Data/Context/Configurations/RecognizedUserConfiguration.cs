using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostIt.Domain.Entities;
using PostIt.Domain.ValueObjects.User;

namespace PostIt.Infrastructure.Data.Context.Configurations;

public class RecognizedUserConfiguration : IEntityTypeConfiguration<RecognizedUser>
{
    public void Configure(EntityTypeBuilder<RecognizedUser> builder)
    {
        builder.HasKey(ru => ru.Id);
        
        builder.OwnsOne(ru => ru.Salt, salt =>
        {
            salt.Property(s => s.Value)
                .IsRequired() 
                .HasMaxLength(Salt.Minlength);
        });
        
        builder.OwnsOne(ru => ru.PasswordHash, passwordHash =>
        {
            passwordHash.Property(p => p.Value)
                .IsRequired() 
                .HasMaxLength(Password.MaxLength);
        });

        builder.HasOne(ru => ru.User)
            .WithOne(u => u.RecognizedUser)
            .HasForeignKey<RecognizedUser>(ru => ru.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.ToTable("RecognizedUsers");
    }
}