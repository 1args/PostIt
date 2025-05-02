namespace PostIt.Application.Abstractions.Data;

public interface IImageProcessor
{
    Task<ReadOnlyMemory<byte>> ResizeImageAsync(ReadOnlyMemory<byte> imageStream, CancellationToken cancellationToken);
}