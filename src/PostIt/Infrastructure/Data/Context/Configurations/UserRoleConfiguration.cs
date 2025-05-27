using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostIt.Domain.Entities;

namespace PostIt.Infrastructure.Data.Context.Configurations;

/// <summary>
/// Configuration of the user role model.
/// </summary>
public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(ur => ur.UserId);

        builder.HasOne<Role>()
            .WithMany()
            .HasForeignKey(ur => ur.RoleId);
        
        builder.ToTable("UserRoles");
    }
}