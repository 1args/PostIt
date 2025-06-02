using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostIt.Contracts.Options;

namespace PostIt.Infrastructure.Extensions;

/// <summary>
/// Extension for configuring SMTP email sender.
/// </summary>
internal static class SmtpExtensions
{
    /// <summary>
    /// Configures SMTP settings and FluentEmail.
    /// </summary>
    public static IServiceCollection AddSmtpConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var smtpOptions = configuration.GetSection(nameof(SmtpOptions)).Get<SmtpOptions>();
    
        services
            .AddFluentEmail(smtpOptions!.SenderEmail, smtpOptions.SenderName)
            .AddSmtpSender(smtpOptions.Host, smtpOptions.Ports);
        
        return services;
    }
}