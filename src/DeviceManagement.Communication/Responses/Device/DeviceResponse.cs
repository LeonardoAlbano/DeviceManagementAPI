using DeviceManagement.Communication.Responses.Customer;

namespace DeviceManagement.Communication.Responses.Device;

public class DeviceResponse
{
    public Guid Id { get; set; }
    public string Serial { get; set; } = string.Empty;
    public string IMEI { get; set; } = string.Empty;
    public DateTime? ActivationDate { get; set; }
    public Guid CustomerId { get; set; }
    public CustomerResponse? Customer { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}