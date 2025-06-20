using DeviceManagement.Domain.Enums;
using DeviceManagement.Communication.Responses.Device;

namespace DeviceManagement.Communication.Responses.Event;

public class EventResponse
{
    public Guid Id { get; set; }
    public EventType Type { get; set; }
    public string TypeDescription => Type.ToString();
    public string? Observations { get; set; }
    public DateTime EventDateTime { get; set; }
    public Guid DeviceId { get; set; }
    public DeviceResponse? Device { get; set; }
    public DateTime CreatedAt { get; set; }
}