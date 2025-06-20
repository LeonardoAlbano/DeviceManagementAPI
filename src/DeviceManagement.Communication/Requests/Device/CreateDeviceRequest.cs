using System.ComponentModel.DataAnnotations;

namespace DeviceManagement.Communication.Requests.Device;

public class CreateDeviceRequest
{
    [Required(ErrorMessage = "Serial is required")]
    [StringLength(100, ErrorMessage = "Serial must have a maximum of 100 characters")]
    public string Serial { get; set; } = string.Empty;

    [Required(ErrorMessage = "IMEI is required")]
    [StringLength(15, MinimumLength = 15, ErrorMessage = "IMEI must have exactly 15 characters")]
    [RegularExpression(@"^\d{15}$", ErrorMessage = "IMEI must contain only numbers")]
    public string IMEI { get; set; } = string.Empty;

    public DateTime? ActivationDate { get; set; }

    [Required(ErrorMessage = "CustomerId is required")]
    public Guid CustomerId { get; set; }
}