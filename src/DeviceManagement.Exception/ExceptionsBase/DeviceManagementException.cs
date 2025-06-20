namespace DeviceManagement.Exception.ExceptionsBase;

public abstract class DeviceManagementException : SystemException
{
    protected DeviceManagementException(string message) : base(message) { }
    
    protected DeviceManagementException(string message, System.Exception innerException) : base(message, innerException) { }
}