using System.ComponentModel.DataAnnotations;
using DeviceManagement.Domain.Enums;

namespace DeviceManagement.Communication.Requests.Event;

public class CreateEventRequest
{
    [Required(ErrorMessage = "Type is required")]
    public EventType Type { get; set; }

    [StringLength(500, ErrorMessage = "Observations must have a maximum of 500 characters")]
    public string? Observations { get; set; }

    public DateTime? EventDateTime { get; set; }

    [Required(ErrorMessage = "DeviceId is required")]
    public Guid DeviceId { get; set; }
}