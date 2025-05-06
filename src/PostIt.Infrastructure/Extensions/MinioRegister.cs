using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using PostIt.Application.Abstractions.Data;
using PostIt.Infrastructure.Data.FileStorage;
using PostIt.Infrastructure.Options;

namespace PostIt.Infrastructure.Extensions;

public static class MinioRegister
{
    public static IServiceCollection AddMinio(
        this IServiceCollection services,
        IConfiguration configuration)
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
        
        return services;
    }
}