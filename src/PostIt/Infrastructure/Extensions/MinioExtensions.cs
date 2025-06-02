using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using PostIt.Application.Abstractions.Data;
using PostIt.Contracts.Options;
using PostIt.Infrastructure.Data.FileStorage;

namespace PostIt.Infrastructure.Extensions;

/// <summary>
/// Extension to configure MinIO client and file storage services.
/// </summary>
internal static class MinioExtensions
{
    /// <summary>
    /// Registers and configures MinIO client, file storage, and image processing services.
    /// </summary>
    public static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(nameof(MinioOptions)));

        services.AddSingleton<IMinioClient>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<MinioOptions>>().Value;

            return new MinioClient()
                .WithEndpoint(options.Endpoint)
                .WithCredentials(options.AccessKey, options.SecretKey)
                .WithSSL(false)
                .Build();
        });

        services.AddScoped<IMinioFileStorage, MinioFileStorage>();
        services.AddScoped<IImageProcessor, ImageProcessor>();
        
        return services;
    }
}