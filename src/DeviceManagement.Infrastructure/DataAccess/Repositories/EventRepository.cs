using Microsoft.EntityFrameworkCore;
using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Repositories;
using DeviceManagement.Domain.Enums;

namespace DeviceManagement.Infrastructure.DataAccess.Repositories;

public class EventRepository : IEventRepository
{
    private readonly DeviceManagementDbContext _context;

    public EventRepository(DeviceManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Event?> GetByIdAsync(Guid id)
    {
        return await _context.Events
            .Include(e => e.Device)
                .ThenInclude(d => d.Customer)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        return await _context.Events
            .Include(e => e.Device)
                .ThenInclude(d => d.Customer)
            .OrderByDescending(e => e.EventDateTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetByDeviceIdAsync(Guid deviceId)
    {
        return await _context.Events
            .Where(e => e.DeviceId == deviceId)
            .Include(e => e.Device)
                .ThenInclude(d => d.Customer)
            .OrderByDescending(e => e.EventDateTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Events
            .Where(e => e.EventDateTime >= startDate && e.EventDateTime <= endDate)
            .Include(e => e.Device)
                .ThenInclude(d => d.Customer)
            .OrderByDescending(e => e.EventDateTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetByDeviceAndPeriodAsync(Guid deviceId, DateTime startDate, DateTime endDate)
    {
        return await _context.Events
            .Where(e => e.DeviceId == deviceId && 
                       e.EventDateTime >= startDate && 
                       e.EventDateTime <= endDate)
            .Include(e => e.Device)
                .ThenInclude(d => d.Customer)
            .OrderByDescending(e => e.EventDateTime)
            .ToListAsync();
    }

    public async Task<Dictionary<EventType, int>> GetEventsLast7DaysAsync()
    {
        var startDate = DateTime.UtcNow.AddDays(-7);
        var endDate = DateTime.UtcNow;

        var events = await _context.Events
            .Where(e => e.EventDateTime >= startDate && e.EventDateTime <= endDate)
            .GroupBy(e => e.Type)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .ToListAsync();

        var result = new Dictionary<EventType, int>();
        
        // Ensure all event types are in the result, even with count 0
        foreach (EventType type in Enum.GetValues<EventType>())
        {
            result[type] = events.FirstOrDefault(e => e.Type == type)?.Count ?? 0;
        }

        return result;
    }

    public async Task<IEnumerable<Event>> GetRecentCriticalEventsAsync(int hours = 24)
    {
        var limitDate = DateTime.UtcNow.AddHours(-hours);

        return await _context.Events
            .Where(e => e.EventDateTime >= limitDate && e.Type == EventType.SignalLoss)
            .Include(e => e.Device)
                .ThenInclude(d => d.Customer)
            .OrderByDescending(e => e.EventDateTime)
            .ToListAsync();
    }

    public async Task AddAsync(Event eventEntity)
    {
        await _context.Events.AddAsync(eventEntity);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Events.AnyAsync(e => e.Id == id);
    }

    public async Task<(IEnumerable<Event> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var query = _context.Events
            .Include(e => e.Device)
                .ThenInclude(d => d.Customer)
            .OrderByDescending(e => e.EventDateTime);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Dictionary<EventType, int>> GetStatisticsByTypeAsync(DateTime startDate, DateTime endDate)
    {
        var events = await _context.Events
            .Where(e => e.EventDateTime >= startDate && e.EventDateTime <= endDate)
            .GroupBy(e => e.Type)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .ToListAsync();

        var result = new Dictionary<EventType, int>();
        
        foreach (EventType type in Enum.GetValues<EventType>())
        {
            result[type] = events.FirstOrDefault(e => e.Type == type)?.Count ?? 0;
        }

        return result;
    }
}
