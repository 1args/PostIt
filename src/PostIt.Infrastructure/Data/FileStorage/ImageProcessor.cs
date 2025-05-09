using Microsoft.Extensions.Options;
using PostIt.Application.Abstractions.Data;
using PostIt.Infrastructure.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace PostIt.Infrastructure.Data.FileStorage;

public class ImageProcessor(IOptions<ImageResizeOptions> options) : IImageProcessor
{
    private readonly ImageResizeOptions _options = options.Value;
    
    public async Task<ReadOnlyMemory<byte>> ResizeImageAsync(
        ReadOnlyMemory<byte> imageStream,
        CancellationToken cancellationToken)
    {
        var outputStream = new MemoryStream(imageStream.ToArray());
        using var image = await Image.LoadAsync(outputStream, cancellationToken);

        var resizeOptions = new ResizeOptions
        {
            Size = new Size(_options.Width, _options.Height),
            Mode = ResizeMode.Crop,
            Position = AnchorPositionMode.Center
        };
        
        image.Mutate(i => i.Resize(resizeOptions));
        
        await image.SaveAsWebpAsync(outputStream, cancellationToken);
        return new ReadOnlyMemory<byte>(outputStream.ToArray());
    }
}