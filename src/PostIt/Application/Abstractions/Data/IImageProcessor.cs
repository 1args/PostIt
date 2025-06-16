namespace PostIt.Application.Abstractions.Data;

/// <summary>
/// Works with image data.
/// </summary>
public interface IImageProcessor
{
    /// <summary>
    /// Resizes an image and returns the new version as bytes.
    /// </summary>
    /// <param name="payload">Original image stream.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Stream containing the resized image.</returns>
    Task<Stream> ResizeImageAsync(Stream payload, CancellationToken cancellationToken);
}