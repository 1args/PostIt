namespace PostIt.Application.Abstractions.Authentication;

/// <summary>
/// Provides functionality for hashing passwords.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Generates a secure hash from the provided plain-text password.
    /// </summary>
    /// <param name="password">Plain-text password to hash.</param>
    /// <returns>Hashed representation of the password.</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifies whether the provided plain-text password matches the hashed password.
    /// </summary>
    /// <param name="password">Plain-text password to verify.</param>
    /// <param name="hashedPassword">Hashed password to compare against.</param>
    /// <returns><c>true</c> if the password matches the hash; otherwise, <c>false</c>.</returns>
    bool VerifyHashedPassword(string password, string hashedPassword);
}