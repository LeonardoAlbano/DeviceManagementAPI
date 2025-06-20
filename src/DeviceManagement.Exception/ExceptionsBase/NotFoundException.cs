namespace DeviceManagement.Exception.ExceptionsBase;

public class NotFoundException : DeviceManagementException
{
    public NotFoundException(string message) : base(message) { }
    
    public NotFoundException(string resourceName, object key) 
        : base($"{resourceName} with identifier '{key}' was not found") { }
}