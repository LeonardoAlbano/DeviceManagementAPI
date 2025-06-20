using DeviceManagement.Domain.Entities;

namespace DeviceManagement.Domain.Repositories;

public interface IDeviceRepository
{
    Task<Device?> GetByIdAsync(Guid id);
    Task<Device?> GetBySerialAsync(string serial);
    Task<Device?> GetByIMEIAsync(string imei);
    Task<IEnumerable<Device>> GetAllAsync();
    Task<IEnumerable<Device>> GetByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<Device>> GetActiveAsync();
    Task AddAsync(Device device);
    Task UpdateAsync(Device device);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> SerialExistsAsync(string serial, Guid? excludeId = null);
    Task<bool> IMEIExistsAsync(string imei, Guid? excludeId = null);
    Task<(IEnumerable<Device> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
}