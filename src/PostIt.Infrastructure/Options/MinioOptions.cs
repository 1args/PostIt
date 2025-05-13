namespace PostIt.Infrastructure.Options;

/// <summary>
/// Configuration options for connecting to a MinIO storage service.
/// </summary>
public class MinioOptions
{
    /// <summary>MinIO server endpoint</summary>
    public string Endpoint { get; set; } = string.Empty;
    
    /// <summary>Access key for authentication.</summary>
    public string AccessKey { get; set; } = string.Empty;
    
    /// <summary>Secret key for authentication.</summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>Name of the bucket where files will be stored.</summary>
    public string BucketName { get; set; } = string.Empty;
}