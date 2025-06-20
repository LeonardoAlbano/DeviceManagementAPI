namespace DeviceManagement.Communication.Responses.Dashboard;

public class DashboardResponse
{
    public Dictionary<string, int> EventsByType { get; set; } = new();
    public int TotalEvents { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<EventTypeStatistic> Statistics { get; set; } = new();
}

public class EventTypeStatistic
{
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal Percentage { get; set; }
    public bool IsCritical { get; set; }
}