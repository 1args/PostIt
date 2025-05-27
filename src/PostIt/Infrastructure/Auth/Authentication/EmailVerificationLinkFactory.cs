using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PostIt.Application.Abstractions.Auth.Authentication;
using PostIt.Domain.Entities;

namespace PostIt.Infrastructure.Auth.Authentication;

/// <inheritdoc/>
public class EmailVerificationLinkFactory(
    IHttpContextAccessor httpContextAccessor,
    LinkGenerator linkGenerator) : IEmailVerificationLinkFactory
{
    /// <inheritdoc/>
    public string Create(EmailVerificationToken emailVerificationToken)
    {
        var httpContext = httpContextAccessor.HttpContext 
                          ?? throw new InvalidOperationException("HTTP context is not available.");
        
        var verificationLink = linkGenerator.GetUriByName(
            httpContext,
            "VerifyEmailAsync",
            new
            {
                userId = emailVerificationToken.UserId, 
                token = emailVerificationToken.Id
            });

        return verificationLink ?? throw new InvalidOperationException("Verification link could not be generated.");
    }
} 