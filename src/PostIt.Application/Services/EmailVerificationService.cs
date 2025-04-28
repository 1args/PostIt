using Hangfire;
using PostIt.Application.Abstractions.Auth;
using PostIt.Application.Abstractions.Data;
using PostIt.Application.Abstractions.Services;
using PostIt.Domain.Entities;

namespace PostIt.Application.Services;

public class EmailVerificationService(
    IRepository<EmailVerificationToken> emailVerificationTokenRepository,
    IEmailVerificationLinkFactory emailVerificationLinkFactory,
    IBackgroundJobClient backgroundJobClient) : IEmailVerificationService
{
    public async Task SendVerificationEmailAsync(User user, CancellationToken cancellationToken)
    {
        var emailVerificationToken = EmailVerificationToken.Create(user.Id, DateTime.UtcNow);
        var verificationLink = emailVerificationLinkFactory.Create(emailVerificationToken);
        
        await emailVerificationTokenRepository.AddAsync(emailVerificationToken, cancellationToken);
        
        backgroundJobClient.Enqueue<IEmailService>(emailService => 
            emailService.SendEmailAsync(
                user.Email.Value,
                "Welcome to the world of notes and creativity - PostIt is waiting for you!",
                $"Congratulations, {user.Name}, your account has been successfully created. " +
                $"Please <a href='{verificationLink}'>click here</a> to confirm your email. " +
                $"The link will expire in 24 hours.",
                cancellationToken,
                true));
    }

    public async Task<bool> VerifyEmailAsync(User user, Guid token, CancellationToken cancellationToken)
    {
        var emailVerificationToken = await emailVerificationTokenRepository.GetByIdAsync(
            t => t.Id == token, cancellationToken);

        if (emailVerificationToken is null || emailVerificationToken.IsExpired())
        {
            return false;
        }
        
        await emailVerificationTokenRepository.DeleteAsync([emailVerificationToken], cancellationToken);
        
        return true;
    }
}