using PostIt.Application.Abstractions.Auth;

namespace PostIt.Infrastructure.Auth;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty.");
        }
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }
    
    public bool VerifyHashedPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty.");
        }
        
        if (string.IsNullOrWhiteSpace(hashedPassword))
        {
            throw new ArgumentException("Hashed password cannot be null.");
        }
        
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}