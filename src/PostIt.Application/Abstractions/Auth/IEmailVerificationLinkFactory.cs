using PostIt.Domain.Entities;

namespace PostIt.Application.Abstractions.Auth;

public interface IEmailVerificationLinkFactory
{
    string Create(EmailVerificationToken emailVerificationToken);
}