using System.ComponentModel.DataAnnotations;

namespace DeviceManagement.Communication.Requests.Customer;

public class UpdateCustomerRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(255, ErrorMessage = "Name must have a maximum of 255 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email")]
    [StringLength(255, ErrorMessage = "Email must have a maximum of 255 characters")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone")]
    [StringLength(20, ErrorMessage = "Phone must have a maximum of 20 characters")]
    public string? Phone { get; set; }

    public bool Status { get; set; }
}