namespace PostIt.Application.Abstractions.Services;

/// <summary>
/// Provides functionality for sending emails.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends a basic email message.
    /// </summary>
    /// <param name="to">Email address of the recipient.</param>
    /// <param name="subject">Subject of the email.</param>
    /// <param name="body">Body content of the email.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <param name="isHtml">Flag indicating whether the email body is in HTML format (default is false).</param>
    Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken, bool isHtml = false);
    
    /// <summary>
    /// Sends a templated email message.
    /// </summary>
    /// <param name="to">Email address of the recipient.</param>
    /// <param name="subject">Subject of the email.</param>
    /// <param name="templateName">Name of the email template to be used.</param>
    /// <param name="model">Model that contains the data to populate the template.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <param name="isHtml">Flag indicating whether the email body is in HTML format (default is false).</param>
    /// <typeparam name="TModel">Type of the model that will be used to populate the template.</typeparam>
    Task SendTemplatedEmailAsync<TModel>(string to, string subject, string templateName, TModel model,
        CancellationToken cancellationToken, bool isHtml = false);
}