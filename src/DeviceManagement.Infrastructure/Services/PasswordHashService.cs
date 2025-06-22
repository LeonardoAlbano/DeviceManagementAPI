using System.Security.Cryptography;
using System.Text;

namespace DeviceManagement.Infrastructure.Services;

public static class PasswordHashService
{
    public static (string Hash, string Salt) GenerateHashAndSalt(string password)
    {
        var salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var hash = HashPassword(password, salt);
        return (hash, salt);
    }

    private static string HashPassword(string password, string salt)
    {
        var combined = password + salt;
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
        return Convert.ToBase64String(hashedBytes);
    }
}