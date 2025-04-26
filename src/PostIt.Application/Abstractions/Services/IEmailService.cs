using FluentEmail.Core.Models;

namespace PostIt.Application.Abstractions.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    
    Task SendTemplatedEmailAsync<TModel>(string to, string subject, string templateName, TModel model);
}