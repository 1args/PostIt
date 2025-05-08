namespace PostIt.Application.Abstractions.Data;

/// <summary>
/// Handles uploading files to MinIO object storage.
/// </summary>
public interface IMinioFileStorage
{
    /// <summary>
    /// Uploads a file to MinIO.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="format">Format or MIME type of the file.</param>
    /// <param name="payload">File's binary content.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task UploadFileAsync(string fileName, string format, ReadOnlyMemory<byte> payload, CancellationToken cancellationToken);
}