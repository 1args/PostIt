using FluentEmail.Core;
using FluentEmail.Core.Models;
using PostIt.Application.Abstractions.Services;

namespace PostIt.Infrastructure.Communication.Email;

public class EmailService(IFluentEmail fluentEmail) : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        await fluentEmail
            .To(to)
            .Subject(subject)
            .Body(body)
            .SendAsync();
    }

    public async Task SendTemplatedEmailAsync<TModel>(string to, string subject, string templatePath, TModel model)
    {
        await fluentEmail
            .To(to)
            .Subject(subject)
            .UsingTemplateFromFile(templatePath, model)
            .SendAsync();
    }
}