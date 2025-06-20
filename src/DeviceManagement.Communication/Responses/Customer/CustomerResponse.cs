namespace DeviceManagement.Communication.Responses.Customer;

public class CustomerResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public bool Status { get; set; }
    public string StatusDescription => Status ? "Active" : "Inactive";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int TotalDevices { get; set; }
}