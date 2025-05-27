using FluentEmail.Core;
using PostIt.Application.Abstractions.Services;

namespace PostIt.Infrastructure.Communication.Email;

public class EmailService(IFluentEmail fluentEmail) : IEmailService
{
    public async Task SendEmailAsync(
        string to,
        string subject,
        string body, 
        CancellationToken cancellationToken,
        bool isHtml = false)
    {
        await fluentEmail
            .To(to)
            .Subject(subject)
            .Body(body, isHtml: isHtml)
            .SendAsync(cancellationToken);
    }

    public async Task SendTemplatedEmailAsync<TModel>(
        string to,
        string subject,
        string templatePath,
        TModel model, 
        CancellationToken cancellationToken,
        bool isHtml = false)
    {
        await fluentEmail
            .To(to)
            .Subject(subject)
            .UsingTemplateFromFile(templatePath, model, isHtml: isHtml)
            .SendAsync(cancellationToken);
    }
}