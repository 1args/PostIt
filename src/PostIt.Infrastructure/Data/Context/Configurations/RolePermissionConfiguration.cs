using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostIt.Infrastructure.Options;
using Permission = PostIt.Domain.Enums.Permission;
using Role = PostIt.Domain.Enums.Role;
using RolePermission = PostIt.Domain.Entities.RolePermission;

namespace PostIt.Infrastructure.Data.Context.Configurations;

public class RolePermissionConfiguration(
    AuthorizationOptions authorizationOptions) : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

        var data = authorizationOptions.Permissions
            .SelectMany(rp => rp.Permissions
                .Select(p => new RolePermission
                {
                    RoleId = (int)Enum.Parse<Role>(rp.Role),
                    PermissionId = (int)Enum.Parse<Permission>(p)
                }))
            .GroupBy(x => new { x.RoleId, x.PermissionId })
            .Select(g => g.First()) // pick the first one from each group
            .ToArray();

        builder.HasData(data);
    }
}