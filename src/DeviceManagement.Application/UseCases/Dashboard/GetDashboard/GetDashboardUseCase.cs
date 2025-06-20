using DeviceManagement.Communication.Responses.Dashboard;
using DeviceManagement.Domain.Repositories;
using DeviceManagement.Domain.Enums;

namespace DeviceManagement.Application.UseCases.Dashboard.GetDashboard;

public class GetDashboardUseCase : IGetDashboardUseCase
{
    private readonly IEventRepository _eventRepository;

    public GetDashboardUseCase(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<DashboardResponse> Execute()
    {
        var eventsByType = await _eventRepository.GetEventsLast7DaysAsync();
        var endDate = DateTime.UtcNow;
        var startDate = endDate.AddDays(-7);
        var totalEvents = eventsByType.Values.Sum();

        var response = new DashboardResponse
        {
            EventsByType = eventsByType.ToDictionary(
                kvp => kvp.Key.ToString(),
                kvp => kvp.Value
            ),
            TotalEvents = totalEvents,
            StartDate = startDate,
            EndDate = endDate,
            Statistics = CreateStatistics(eventsByType, totalEvents)
        };

        return response;
    }

    private static List<EventTypeStatistic> CreateStatistics(
        Dictionary<EventType, int> eventsByType,
        int totalEvents)
    {
        var statistics = new List<EventTypeStatistic>();

        foreach (var kvp in eventsByType)
        {
            var percentage = totalEvents > 0 ? (decimal)kvp.Value / totalEvents * 100 : 0;

            statistics.Add(new EventTypeStatistic
            {
                Type = kvp.Key.ToString(),
                Description = kvp.Key.GetDescription(),
                Count = kvp.Value,
                Percentage = Math.Round(percentage, 2),
                IsCritical = kvp.Key.IsCritical()
            });
        }

        return statistics.OrderByDescending(e => e.Count).ToList();
    }
}