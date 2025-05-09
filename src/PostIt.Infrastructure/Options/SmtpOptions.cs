namespace PostIt.Infrastructure.Options;

public class SmtpOptions
{
    public string Host { get; set; } = string.Empty;
    
    public int Ports { get; set; }
    
    public string SenderName { get; set; } = string.Empty;
    
    public string SenderEmail { get; set; } = string.Empty;
    
    public string SenderPassword { get; set; } = string.Empty;

}