namespace DeviceManagement.Exception.ExceptionsBase;

public class ConflictException : DeviceManagementException
{
    public ConflictException(string message) : base(message) { }
    
    public ConflictException(string resourceName, string conflictReason) 
        : base($"Conflict in {resourceName}: {conflictReason}") { }
}