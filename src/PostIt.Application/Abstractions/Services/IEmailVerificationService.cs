using PostIt.Domain.Entities;

namespace PostIt.Application.Abstractions.Services;

public interface IEmailVerificationService
{
    Task SendVerificationEmailAsync(User user, CancellationToken cancellationToken);
    
    Task<bool> VerifyEmailAsync(User user, Guid token, CancellationToken cancellationToken);
}