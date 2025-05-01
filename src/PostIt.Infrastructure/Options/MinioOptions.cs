using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PostIt.Infrastructure.Options;

public class MinioOptions
{
    public string AccessKey { get; set; } = string.Empty;
    
    public string SecretKey { get; set; } = string.Empty;

    public string BucketName { get; set; } = string.Empty;
}