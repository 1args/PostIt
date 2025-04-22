namespace PostIt.Application.Abstractions.Auth;

public interface IPasswordHasher
{
    string HashPassword(string password);

    bool VerifyHashedPassword(string password, string hashedPassword);

    string GenerateSalt();
}