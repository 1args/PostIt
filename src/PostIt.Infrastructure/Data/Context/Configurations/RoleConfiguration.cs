using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostIt.Domain.Entities;

namespace PostIt.Infrastructure.Data.Context.Configurations;

/// <summary>
/// Configuration of the role model.
/// </summary>
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasMany(r => r.Users)
            .WithMany(u => u.Roles)
            .UsingEntity<UserRole>();

        // Налаштування Role-Permission
        builder.HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity<RolePermission>(
                l => l.HasOne<Permission>().WithMany().HasForeignKey(rp => rp.PermissionId),
                r => r.HasOne<Role>().WithMany().HasForeignKey(rp => rp.RoleId))
            .ToTable("RolePermissions");
        
        var roles = Enum.GetValues<Domain.Enums.Role>()
            .Select(r => Role.Create((int)r, r.ToString()));
        
        builder.HasData(roles);
        
        builder.ToTable("Roles");
    }
}