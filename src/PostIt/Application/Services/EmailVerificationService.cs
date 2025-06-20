using Microsoft.EntityFrameworkCore;
using PostIt.Application.Abstractions.Data;
using PostIt.Application.Abstractions.Security.Authentication;
using PostIt.Application.Abstractions.Services;
using PostIt.Domain.Entities;

namespace PostIt.Application.Services;

/// <inheritdoc/>
public class EmailVerificationService(
    IRepository<EmailVerificationToken> emailVerificationTokenRepository,
    IEmailService emailService,
    IEmailVerificationLinkFactory emailVerificationLinkFactory) : IEmailVerificationService
{
    /// <inheritdoc/>
    public async Task SendVerificationEmailAsync(User user, CancellationToken cancellationToken)
    {
        var emailVerificationToken = EmailVerificationToken.Create(user.Id, DateTime.UtcNow);
        
        await emailVerificationTokenRepository.AddAsync(emailVerificationToken, cancellationToken);
        
        var verificationLink = emailVerificationLinkFactory.Create(emailVerificationToken);

        await emailService.SendEmailAsync(
            user.Email.Value,
            "Welcome to the world of notes and creativity - PostIt is waiting for you!",
            $"Congratulations, <b>{user.Name}</b>, your account has been successfully created. " +
            $"Please <a href='{verificationLink}'>click here</a> to confirm your email. " +
            $"The link will expire in <b>24 hours</b>.",
            cancellationToken,
            true);
    }

    /// <inheritdoc/>
    public async Task<bool> VerifyEmailAsync(User user, Guid token, CancellationToken cancellationToken)
    {
        var emailVerificationToken = await emailVerificationTokenRepository
            .AsQueryable()
            .SingleOrDefaultAsync(e => e.Id == token,cancellationToken);

        if (emailVerificationToken is null || emailVerificationToken.IsExpired())
        {
            return false;
        }
        
        await emailVerificationTokenRepository.DeleteAsync([emailVerificationToken], cancellationToken);
        
        return true;
    }
}