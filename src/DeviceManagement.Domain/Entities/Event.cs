using DeviceManagement.Domain.Entities.Base;
using DeviceManagement.Domain.Enums;

namespace DeviceManagement.Domain.Entities;

public class Event : BaseEntity
{
    public EventType Type { get; private set; }
    public string? Observations { get; private set; }
    public DateTime EventDateTime { get; private set; }
    public Guid DeviceId { get; private set; }
    public virtual Device Device { get; private set; } = null!;

    private Event() { }

    public Event(EventType type, Guid deviceId, string? observations = null, DateTime? eventDateTime = null)
    {
        Type = type;
        DeviceId = deviceId;
        Observations = observations;
        EventDateTime = eventDateTime ?? DateTime.UtcNow;

        ValidateEntity();
    }

    public void UpdateObservations(string? observations)
    {
        Observations = observations;
        UpdateModificationDate();

        ValidateObservations();
    }

    public bool IsCritical()
    {
        return Type.IsCritical();
    }

    public string GetTypeDescription()
    {
        return Type.GetDescription();
    }

    public bool OccurredInPeriod(DateTime startDate, DateTime endDate)
    {
        return EventDateTime >= startDate && EventDateTime <= endDate;
    }

    public bool IsRecent(int hours = 1)
    {
        var limitDate = DateTime.UtcNow.AddHours(-hours);
        return EventDateTime >= limitDate;
    }

    public double MinutesSinceEvent()
    {
        return (DateTime.UtcNow - EventDateTime).TotalMinutes;
    }

    private void ValidateEntity()
    {
        var errors = new List<string>();

        if (!Enum.IsDefined(typeof(EventType), Type))
            errors.Add("Invalid event type");

        if (DeviceId == Guid.Empty)
            errors.Add("DeviceId is required");

        if (EventDateTime > DateTime.UtcNow.AddMinutes(5))
            errors.Add("EventDateTime cannot be in the future");

        ValidateObservations();

        if (errors.Any())
            throw new ArgumentException($"Validation error: {string.Join(", ", errors)}");
    }

    private void ValidateObservations()
    {
        if (!string.IsNullOrEmpty(Observations) && Observations.Length > 500)
            throw new ArgumentException("Observations must have a maximum of 500 characters");
    }

    public override string ToString()
    {
        var observations = string.IsNullOrEmpty(Observations) ? "" : $" - {Observations}";
        return $"Event: {GetTypeDescription()} at {EventDateTime:dd/MM/yyyy HH:mm:ss}{observations}";
    }
}
