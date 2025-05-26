namespace PostIt.Contracts.Options;

/// <summary>
/// Configuration options for image resizing.
/// </summary>
public class ImageResizeOptions
{
    /// <summary>Width to resize the image to (in pixels).</summary>
    public int Width { get; set; } = 600;
    
    /// <summary>Height to resize the image to (in pixels).</summary>
    public int Height { get; set; } = 600;
}