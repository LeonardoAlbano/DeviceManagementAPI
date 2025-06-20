using DeviceManagement.Domain.Entities;

namespace DeviceManagement.Domain.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id);
    Task<Customer?> GetByEmailAsync(string email);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<IEnumerable<Customer>> GetActiveAsync();
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> EmailExistsAsync(string email, Guid? excludeId = null);
    Task<(IEnumerable<Customer> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
}