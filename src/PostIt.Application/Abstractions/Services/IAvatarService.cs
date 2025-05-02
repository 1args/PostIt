namespace PostIt.Application.Abstractions.Services;

public interface IAvatarService
{
    Task<string> UploadAvatarAsync(Guid userId, ReadOnlyMemory<byte> avatar, CancellationToken cancellationToken);
}