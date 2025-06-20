namespace DeviceManagement.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task Commit();
    Task Rollback();
    Task BeginTransaction();
}