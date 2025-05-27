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
    public async Task<ReadOnlyMemory<byte>> ResizeImageAsync(
        ReadOnlyMemory<byte> imageStream,
        CancellationToken cancellationToken)
    {
        using var image = Image.Load(imageStream.Span); 
    
        var resizeOptions = new ResizeOptions
        {
            Size = new Size(_options.Width, _options.Height),
            Mode = ResizeMode.Crop,
            Position = AnchorPositionMode.Center
        };
    
        image.Mutate(i => i.Resize(resizeOptions));
        
        using var outputStream = new MemoryStream();
        await image.SaveAsWebpAsync(outputStream, cancellationToken);
    
        return outputStream.ToArray();
    }
}