namespace DeviceManagement.Domain.Enums;

public enum EventType
{
    TurnedOn = 0,
    
    TurnedOff = 1,
    
    Movement = 2,
    
    SignalLoss = 3
}

public static class EventTypeExtensions
{
    public static string GetDescription(this EventType type)
    {
        return type switch
        {
            EventType.TurnedOn => "Device Turned On",
            EventType.TurnedOff => "Device Turned Off",
            EventType.Movement => "Movement Detected",
            EventType.SignalLoss => "Signal Loss",
            _ => "Unknown Type"
        };
    }
    
    public static bool IsCritical(this EventType type)
    {
        return type == EventType.SignalLoss;
    }
}