using Microsoft.Extensions.Options;
using PostIt.Application.Abstractions.Data;
using PostIt.Contracts.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace PostIt.Infrastructure.Data.FileStorage;

/// <inheritdoc/>
public class ImageProcessor(
    IOptions<ImageResizeOptions> options) : IImageProcessor
{
    private readonly ImageResizeOptions _options = options.Value;
    
    /// <inheritdoc/>
    public async Task<Stream> ResizeImageAsync(
        Stream payload,
        CancellationToken cancellationToken)
    {
        if (payload.CanSeek)
        {
            payload.Seek(0, SeekOrigin.Begin);
        }
        
        using var image = await Image.LoadAsync(payload, cancellationToken); 
    
        var resizeOptions = new ResizeOptions
        {
            Size = new Size(_options.Width, _options.Height),
            Mode = ResizeMode.Crop,
            Position = AnchorPositionMode.Center
        };
    
        image.Mutate(i => i.Resize(resizeOptions));
        
        var outputStream = new MemoryStream();
        await image.SaveAsWebpAsync(outputStream, cancellationToken);
    
        return outputStream;
    }
}