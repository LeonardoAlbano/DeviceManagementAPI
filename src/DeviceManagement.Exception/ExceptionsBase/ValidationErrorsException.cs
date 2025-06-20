namespace DeviceManagement.Exception.ExceptionsBase;

public class ValidationErrorsException : DeviceManagementException
{
    public List<string> ErrorMessages { get; set; }

    public ValidationErrorsException(List<string> errorMessages) 
        : base(string.Join("; ", errorMessages))
    {
        ErrorMessages = errorMessages;
    }

    public ValidationErrorsException(string errorMessage) 
        : base(errorMessage)
    {
        ErrorMessages = new List<string> { errorMessage };
    }
}