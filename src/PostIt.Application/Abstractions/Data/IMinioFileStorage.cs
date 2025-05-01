namespace PostIt.Application.Abstractions.Data;

public interface IMinioFileStorage
{
    Task UploadFileAsync(string fileName, string format, ReadOnlyMemory<byte> payload, CancellationToken cancellationToken);
}