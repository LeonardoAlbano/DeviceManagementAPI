using Microsoft.EntityFrameworkCore.Storage;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Infrastructure.DataAccess;
public class UnitOfWork : IUnitOfWork
{
    private readonly DeviceManagementDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(DeviceManagementDbContext context)
    {
        _context = context;
    }

    public async Task Commit()
    {
        try
        {
            await _context.SaveChangesAsync();
            
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        catch
        {
            await Rollback();
            throw;
        }
    }

    public async Task Rollback()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task BeginTransaction()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}