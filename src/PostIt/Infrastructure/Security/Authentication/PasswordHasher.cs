using PostIt.Application.Abstractions.Security.Authentication;

namespace PostIt.Infrastructure.Security.Authentication;

/// <inheritdoc/>
public class PasswordHasher : IPasswordHasher
{
    /// <inheritdoc/>
    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be empty.");
        }
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }
    
    /// <inheritdoc/>
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