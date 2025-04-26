using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostIt.Infrastructure.Options;

namespace PostIt.Infrastructure.Extensions;

public static class SmtpRegister
{
    public static IServiceCollection AddSmtpConfiguration(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var smtpOptions = configuration.GetSection(nameof(SmtpOptions)).Get<SmtpOptions>();
    
        services
            .AddFluentEmail(smtpOptions!.SenderEmail, smtpOptions.SenderName)
            .AddSmtpSender(smtpOptions.Host, smtpOptions.Ports);
        
        return services;
    }
}