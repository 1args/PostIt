namespace PostIt.Application.Abstractions.Data;

/// <summary>
/// Works with image data.
/// </summary>
public interface IImageProcessor
{
    /// <summary>
    /// Resizes an image and returns the new version as bytes.
    /// </summary>
    /// <param name="imageStream">Original image data.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Resized image in byte form.</returns>
    Task<ReadOnlyMemory<byte>> ResizeImageAsync(ReadOnlyMemory<byte> imageStream, CancellationToken cancellationToken);
}