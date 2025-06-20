using DeviceManagement.Domain.Entities.Base;
using System.Text.RegularExpressions;

namespace DeviceManagement.Domain.Entities;

public class Device : BaseEntity
{
    public string Serial { get; private set; } = string.Empty;
    public string IMEI { get; private set; } = string.Empty;
    public DateTime? ActivationDate { get; private set; }
    public Guid CustomerId { get; private set; }
    public virtual Customer Customer { get; private set; } = null!;
    public virtual ICollection<Event> Events { get; private set; } = new List<Event>();

    private Device() { }

    public Device(string serial, string imei, Guid customerId, DateTime? activationDate = null)
    {
        Serial = serial;
        IMEI = imei;
        CustomerId = customerId;
        ActivationDate = activationDate;

        ValidateEntity();
    }

    public void UpdateData(string serial, string imei, DateTime? activationDate = null)
    {
        Serial = serial;
        IMEI = imei;
        ActivationDate = activationDate;
        UpdateModificationDate();

        ValidateEntity();
    }

    public void Activate()
    {
        ActivationDate = DateTime.UtcNow;
        UpdateModificationDate();
    }

    public void Deactivate()
    {
        ActivationDate = null;
        UpdateModificationDate();
    }

    public bool IsActive()
    {
        return ActivationDate.HasValue;
    }

    public Event? LastEvent()
    {
        return Events
            .OrderByDescending(e => e.EventDateTime)
            .FirstOrDefault();
    }

    public IEnumerable<Event> GetEventsByPeriod(DateTime startDate, DateTime endDate)
    {
        return Events
            .Where(e => e.EventDateTime >= startDate && e.EventDateTime <= endDate)
            .OrderByDescending(e => e.EventDateTime);
    }

    public int CountEventsByType(Domain.Enums.EventType type)
    {
        return Events.Count(e => e.Type == type);
    }

    public bool HasRecentActivity(int hours = 24)
    {
        var limitDate = DateTime.UtcNow.AddHours(-hours);
        return Events.Any(e => e.EventDateTime >= limitDate);
    }

    private void ValidateEntity()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(Serial))
            errors.Add("Serial is required");

        if (Serial?.Length > 100)
            errors.Add("Serial must have a maximum of 100 characters");

        if (string.IsNullOrWhiteSpace(IMEI))
            errors.Add("IMEI is required");

        if (!IsValidIMEI(IMEI))
            errors.Add("IMEI must have exactly 15 numeric digits");

        if (CustomerId == Guid.Empty)
            errors.Add("CustomerId is required");

        if (errors.Any())
            throw new ArgumentException($"Validation error: {string.Join(", ", errors)}");
    }

    private static bool IsValidIMEI(string? imei)
    {
        if (string.IsNullOrWhiteSpace(imei))
            return false;

        return imei.Length == 15 && Regex.IsMatch(imei, @"^\d{15}$");
    }

    public override string ToString()
    {
        var status = IsActive() ? "Active" : "Inactive";
        return $"Device: {Serial} (IMEI: {IMEI}) - Status: {status}";
    }
}
