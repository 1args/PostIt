using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using PostIt.Application.Abstractions.Data;
using PostIt.Infrastructure.Options;

namespace PostIt.Infrastructure.Data.FileStorage;

public class MinioFileStorage(
    IMinioClient minioClient,
    IOptions<MinioOptions> options,
    ILogger<MinioFileStorage> logger) : IMinioFileStorage
{
    private readonly MinioOptions _options = options.Value;

    public async Task UploadFileAsync(
        string fileName,
        string format, 
        ReadOnlyMemory<byte> payload,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting upload for file `{FileName}` with format `{Format}`.", fileName, format);

        try
        {
            await EnsureBucketExitsAsync(cancellationToken);
        
            using var stream = new MemoryStream(payload.ToArray());

            var uploadArgs = new PutObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject(fileName)
                .WithStreamData(stream)
                .WithObjectSize(payload.Length)
                .WithContentType(format);

            await minioClient.PutObjectAsync(uploadArgs, cancellationToken);
            
            logger.LogInformation(
                "Successfully uploaded file `{FileName}` to bucket `{Bucket}`.", 
                fileName,
                _options.BucketName);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Failed to upload file `{FileName}` to bucket `{Bucket}`.",
                fileName,
                _options.BucketName);;
            throw;
        }
    }

    private async Task EnsureBucketExitsAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Checking if bucket `{Bucket}` exists...", _options.BucketName);

        try
        {
            var bucketExistsArgs = new BucketExistsArgs()
                .WithBucket(_options.BucketName);
        
            var exists = await minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);

            if (!exists)
            {
                logger.LogWarning("Bucket `{Bucket}` does not exist. Creating...", _options.BucketName);
                
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(_options.BucketName);
            
                await minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                
                logger.LogInformation("Bucket `{Bucket}` created successfully.", _options.BucketName);
            }
            else
            {
                logger.LogInformation("Bucket `{Bucket}` already exists.", _options.BucketName);
            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error while ensuring bucket `{Bucket}` exists.", _options.BucketName);
            throw;
        }
    }
}