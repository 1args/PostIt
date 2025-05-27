namespace PostIt.Contracts.Options;

/// <summary>
/// Configuration options for sending emails via SMTP.
/// </summary>
public class SmtpOptions
{
    /// <summary>SMTP server host.</summary>
    public string Host { get; set; } = string.Empty;
    
    /// <summary>SMTP port number.</summary>
    public int Ports { get; set; }
    
    /// <summary>Name of the email sender.</summary>
    public string SenderName { get; set; } = string.Empty;
    
    /// <summary>Email address of the sender.</summary>
    public string SenderEmail { get; set; } = string.Empty;
    
    /// <summary>Password for the sender's email account.</summary>
    public string SenderPassword { get; set; } = string.Empty;

}