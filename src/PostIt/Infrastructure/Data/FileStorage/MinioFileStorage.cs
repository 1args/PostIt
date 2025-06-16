using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using PostIt.Application.Abstractions.Data;
using PostIt.Contracts.Exceptions;
using PostIt.Contracts.Options;

namespace PostIt.Infrastructure.Data.FileStorage;

/// <inheritdoc/>
public class MinioFileStorage(
    IMinioClient minioClient,
    IOptions<MinioOptions> options,
    ILogger<MinioFileStorage> logger) : IMinioFileStorage
{
    private readonly MinioOptions _options = options.Value;

    /// <inheritdoc/>
    public async Task UploadFileAsync(
        string fileName,
        string format, 
        Stream payload,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting upload for file `{FileName}` with format `{Format}`.", fileName, format);

        try
        {
            await EnsureBucketExistsAsync(cancellationToken);
            
            if (payload.CanSeek)
            {
                payload.Seek(0, SeekOrigin.Begin);
            }
            
            var uploadArgs = new PutObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject(fileName)
                .WithStreamData(payload)
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
            throw new ReadableException("Unknown error occurred while uploading a file.");
        }
    }
    
    /// <inheritdoc/>
    public async Task<Stream> DownloadFileAsync(
        string fileName,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Starting retrieval of file `{FileName}` from bucket `{Bucket}`.",
            fileName,
            _options.BucketName);

        try
        {
            await EnsureBucketExistsAsync(cancellationToken);

            var memoryStream = new MemoryStream();

            var getObjectArgs = new GetObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject(fileName)
                .WithCallbackStream(async (stream, ct) =>
                {
                    await stream.CopyToAsync(memoryStream, ct);
                });

            await minioClient.GetObjectAsync(getObjectArgs, cancellationToken);
            
            return memoryStream;
        }
        catch (ObjectNotFoundException exception)
        {
            throw new NotFoundException($"File '{fileName}' not found in bucket '{_options.BucketName}'.");
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Failed to retrieve file `{FileName}` from bucket `{Bucket}`.",
                fileName,
                _options.BucketName);
            throw new ReadableException("Unknown error occurred while receiving a file.");
        }
    }

    private async Task EnsureBucketExistsAsync(CancellationToken cancellationToken)
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
            logger.LogError
                (exception, 
                    "Error while ensuring bucket `{Bucket}` exists.",
                    _options.BucketName);
            throw new InvalidOperationException(
                $"Unknown error occurred while checking for the existence of the `{_options.BucketName}` bucket.");
        }
    }
}