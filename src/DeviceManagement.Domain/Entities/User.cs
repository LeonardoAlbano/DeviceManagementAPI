using DeviceManagement.Domain.Entities.Base;
using System.Security.Cryptography;
using System.Text;

namespace DeviceManagement.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Salt { get; private set; } = string.Empty;
    public string Role { get; private set; } = "User";
    public bool IsActive { get; private set; } = true;

    private User() { }

    public User(string name, string email, string password, string role = "User")
    {
        Name = name;
        Email = email;
        Role = role;
        SetPassword(password);
        ValidateEntity();
    }

    public void SetPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            throw new ArgumentException("Password must have at least 6 characters");

        Salt = GenerateSalt();
        PasswordHash = HashPassword(password, Salt);
        UpdateModificationDate();
    }

    public bool VerifyPassword(string password)
    {
        var hash = HashPassword(password, Salt);
        return hash == PasswordHash;
    }

    public void Activate()
    {
        IsActive = true;
        UpdateModificationDate();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdateModificationDate();
    }

    public void UpdateData(string name, string email)
    {
        Name = name;
        Email = email;
        UpdateModificationDate();
        ValidateEntity();
    }

    private static string GenerateSalt()
    {
        var bytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    private static string HashPassword(string password, string salt)
    {
        var combined = password + salt;
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
        return Convert.ToBase64String(hashedBytes);
    }

    private void ValidateEntity()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(Name))
            errors.Add("Name is required");

        if (string.IsNullOrWhiteSpace(Email))
            errors.Add("Email is required");

        if (!IsValidEmail(Email))
            errors.Add("Invalid email");

        if (errors.Any())
            throw new ArgumentException($"Validation error: {string.Join(", ", errors)}");
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
