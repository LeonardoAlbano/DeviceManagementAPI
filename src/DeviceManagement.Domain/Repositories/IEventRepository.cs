using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Enums;

namespace DeviceManagement.Domain.Repositories;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(Guid id);
    Task<IEnumerable<Event>> GetAllAsync();
    Task<IEnumerable<Event>> GetByDeviceIdAsync(Guid deviceId);
    Task<IEnumerable<Event>> GetByPeriodAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Event>> GetByDeviceAndPeriodAsync(Guid deviceId, DateTime startDate, DateTime endDate);
    Task<Dictionary<EventType, int>> GetEventsLast7DaysAsync();
    Task<IEnumerable<Event>> GetRecentCriticalEventsAsync(int hours = 24);
    Task AddAsync(Event eventEntity);
    Task<bool> ExistsAsync(Guid id);
    Task<(IEnumerable<Event> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
    Task<Dictionary<EventType, int>> GetStatisticsByTypeAsync(DateTime startDate, DateTime endDate);
}