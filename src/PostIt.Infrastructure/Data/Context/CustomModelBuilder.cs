using Microsoft.EntityFrameworkCore;
using PostIt.Infrastructure.Data.Context.Configurations;

namespace PostIt.Infrastructure.Data.Context;

public static class CustomModelBuilder
{
    public static void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RecognizedUserConfiguration());
        modelBuilder.ApplyConfiguration(new PostConfiguration());
        modelBuilder.ApplyConfiguration(new PostLikeConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new CommentLikeConfiguration());
    }
}