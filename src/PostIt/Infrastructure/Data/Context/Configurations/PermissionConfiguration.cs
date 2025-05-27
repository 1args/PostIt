using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostIt.Domain.Entities;

namespace PostIt.Infrastructure.Data.Context.Configurations;

/// <summary>
/// Configuration of the permission model.
/// </summary>
public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.HasMany(p => p.Roles)
            .WithMany(r => r.Permissions)
            .UsingEntity<RolePermission>(
                l => l.HasOne<Role>().WithMany().HasForeignKey(r => r.RoleId),
                r => r.HasOne<Permission>().WithMany().HasForeignKey(p => p.PermissionId));

        var permissions = Enum.GetValues<Domain.Enums.Permission>()
            .Select(p => Permission.Create((int)p, p.ToString()));

        builder.HasData(permissions);
    }
}