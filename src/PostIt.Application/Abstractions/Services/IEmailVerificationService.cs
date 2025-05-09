using PostIt.Domain.Entities;

namespace PostIt.Application.Abstractions.Services;

/// <summary>
/// Provides functionality for email verification.
/// </summary>
public interface IEmailVerificationService
{
    /// <summary>
    /// Sends a verification email to the specified user.
    /// </summary>
    /// <param name="user">User to whom the verification email will be sent.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task SendVerificationEmailAsync(User user, CancellationToken cancellationToken);
    
    /// <summary>
    /// Verifies a user's email address using a provided verification token.
    /// </summary>
    /// <param name="user">User whose email will be verified.</param>
    /// <param name="token">Verification token that is used to verify the email.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>Indicating whether the email verification was successful.</returns>
    Task<bool> VerifyEmailAsync(User user, Guid token, CancellationToken cancellationToken);
}