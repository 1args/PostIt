using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostIt.Contracts.Options;
using Permission = PostIt.Domain.Enums.Permission;
using Role = PostIt.Domain.Enums.Role;
using RolePermission = PostIt.Domain.Entities.RolePermission;

namespace PostIt.Infrastructure.Data.Context.Configurations;

/// <summary>
/// Configuration of the role permission model.
/// </summary>
public class RolePermissionConfiguration(
    AuthorizationOptions authorizationOptions) : IEntityTypeConfiguration<RolePermission>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

        var data = authorizationOptions.Permissions
            .SelectMany(rp => rp.Permissions
                .Select(p => RolePermission.Create(
                    (int)Enum.Parse<Role>(rp.Role), 
                    (int)Enum.Parse<Permission>(p))))
            .ToArray(); 

        builder.HasData(data);
        
        builder.ToTable("RolePermissions");
    }
}