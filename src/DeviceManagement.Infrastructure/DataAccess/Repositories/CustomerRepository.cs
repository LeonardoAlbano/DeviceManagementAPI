using Microsoft.EntityFrameworkCore;
using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Infrastructure.DataAccess.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly DeviceManagementDbContext _context;

    public CustomerRepository(DeviceManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        return await _context.Customers
            .Include(c => c.Devices)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _context.Customers
            .Include(c => c.Devices)
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .Include(c => c.Devices)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Customer>> GetActiveAsync()
    {
        return await _context.Customers
            .Where(c => c.Status)
            .Include(c => c.Devices)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
    }

    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Customers.AnyAsync(c => c.Id == id);
    }

    public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null)
    {
        var query = _context.Customers.Where(c => c.Email == email);
        
        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<(IEnumerable<Customer> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var query = _context.Customers
            .Include(c => c.Devices)
            .OrderBy(c => c.Name);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
