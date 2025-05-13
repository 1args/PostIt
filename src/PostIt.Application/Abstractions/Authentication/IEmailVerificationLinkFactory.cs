using PostIt.Domain.Entities;

namespace PostIt.Application.Abstractions.Authentication;

/// <summary>
/// A factory for creating email verification links.
/// </summary>
public interface IEmailVerificationLinkFactory
{
    /// <summary>
    /// Creates a verification link for the specified email verification token.
    /// </summary>
    /// <param name="emailVerificationToken">Token containing verification data.</param>
    /// <returns>String representing the email verification link.</returns>
    string Create(EmailVerificationToken emailVerificationToken);
}