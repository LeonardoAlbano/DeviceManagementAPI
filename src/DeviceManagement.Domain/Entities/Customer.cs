using DeviceManagement.Domain.Entities.Base;
using System.Net.Mail;

namespace DeviceManagement.Domain.Entities;

public class Customer : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Phone { get; private set; }
    public bool Status { get; private set; }
    public virtual ICollection<Device> Devices { get; private set; } = new List<Device>();

    private Customer() { }

    public Customer(string name, string email, string? phone = null, bool status = true)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Status = status;

        ValidateEntity();
    }

    public void UpdateData(string name, string email, string? phone = null)
    {
        Name = name;
        Email = email;
        Phone = phone;
        UpdateModificationDate();

        ValidateEntity();
    }

    public void Activate()
    {
        Status = true;
        UpdateModificationDate();
    }

    public void Deactivate()
    {
        Status = false;
        UpdateModificationDate();
    }

    public bool HasActiveDevices()
    {
        return Devices.Any(d => d.IsActive());
    }

    public int TotalDevices => Devices.Count;

    private void ValidateEntity()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(Name))
            errors.Add("Name is required");

        if (Name?.Length > 255)
            errors.Add("Name must have a maximum of 255 characters");

        if (string.IsNullOrWhiteSpace(Email))
            errors.Add("Email is required");

        if (Email?.Length > 255)
            errors.Add("Email must have a maximum of 255 characters");

        if (!IsValidEmail(Email))
            errors.Add("Invalid email");

        if (!string.IsNullOrEmpty(Phone) && Phone.Length > 20)
            errors.Add("Phone must have a maximum of 20 characters");

        if (errors.Any())
            throw new ArgumentException($"Validation error: {string.Join(", ", errors)}");
    }

    private static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public override string ToString()
    {
        return $"Customer: {Name} ({Email}) - Status: {(Status ? "Active" : "Inactive")}";
    }
}
