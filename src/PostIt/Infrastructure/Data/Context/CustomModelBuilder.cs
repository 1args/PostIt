using Microsoft.EntityFrameworkCore;
using PostIt.Infrastructure.Data.Context.Configurations;

namespace PostIt.Infrastructure.Data.Context;

/// <summary>
/// A static class responsible for accepting model configurations.
/// </summary>
public static class CustomModelBuilder
{
    /// <summary>
    /// Configuring models during their creation.
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new EmailVerificationTokenConfiguration());
        modelBuilder.ApplyConfiguration(new PostConfiguration());
        modelBuilder.ApplyConfiguration(new PostLikeConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new CommentLikeConfiguration());
    }
}