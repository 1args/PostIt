using FluentEmail.Core.Models;

namespace PostIt.Application.Abstractions.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken, bool isHtml = false);
    
    Task SendTemplatedEmailAsync<TModel>(string to, string subject, string templateName, TModel model,
        CancellationToken cancellationToken, bool isHtml = false);
}