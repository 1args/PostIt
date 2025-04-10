namespace PostIt.Domain.ValueObjects;

public class Password
{
    public const int MinPasswordLength = 8;
    public const int MaxPasswordLength = 16;
    public string Value { get; }

    private Password(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < MinPasswordLength)
        {
            throw new ArgumentException($"Password must be at least {MinPasswordLength} characters long.", 
                nameof(password));
        }

        if (password.Length > MaxPasswordLength)
        {
            throw new ArgumentException($"Password must be less than {MaxPasswordLength}.", 
                nameof(password));
        }
            
        Value = password;
    }

    public static Password Create(string password) => new(password);
}