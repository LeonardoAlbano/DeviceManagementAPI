using Microsoft.EntityFrameworkCore;
using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Infrastructure.DataAccess.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly DeviceManagementDbContext _context;

    public DeviceRepository(DeviceManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Device?> GetByIdAsync(Guid id)
    {
        return await _context.Devices
            .Include(d => d.Customer)
            .Include(d => d.Events.OrderByDescending(e => e.EventDateTime).Take(10))
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Device?> GetBySerialAsync(string serial)
    {
        return await _context.Devices
            .Include(d => d.Customer)
            .FirstOrDefaultAsync(d => d.Serial == serial);
    }

    public async Task<Device?> GetByIMEIAsync(string imei)
    {
        return await _context.Devices
            .Include(d => d.Customer)
            .FirstOrDefaultAsync(d => d.IMEI == imei);
    }

    public async Task<IEnumerable<Device>> GetAllAsync()
    {
        return await _context.Devices
            .Include(d => d.Customer)
            .OrderBy(d => d.Serial)
            .ToListAsync();
    }

    public async Task<IEnumerable<Device>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.Devices
            .Where(d => d.CustomerId == customerId)
            .Include(d => d.Customer)
            .OrderBy(d => d.Serial)
            .ToListAsync();
    }

    public async Task<IEnumerable<Device>> GetActiveAsync()
    {
        return await _context.Devices
            .Where(d => d.ActivationDate.HasValue)
            .Include(d => d.Customer)
            .OrderBy(d => d.Serial)
            .ToListAsync();
    }

    public async Task AddAsync(Device device)
    {
        await _context.Devices.AddAsync(device);
    }

    public async Task UpdateAsync(Device device)
    {
        _context.Devices.Update(device);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var device = await _context.Devices.FindAsync(id);
        if (device != null)
        {
            _context.Devices.Remove(device);
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Devices.AnyAsync(d => d.Id == id);
    }

    public async Task<bool> SerialExistsAsync(string serial, Guid? excludeId = null)
    {
        var query = _context.Devices.Where(d => d.Serial == serial);
        
        if (excludeId.HasValue)
        {
            query = query.Where(d => d.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<bool> IMEIExistsAsync(string imei, Guid? excludeId = null)
    {
        var query = _context.Devices.Where(d => d.IMEI == imei);
        
        if (excludeId.HasValue)
        {
            query = query.Where(d => d.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<(IEnumerable<Device> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var query = _context.Devices
            .Include(d => d.Customer)
            .OrderBy(d => d.Serial);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
