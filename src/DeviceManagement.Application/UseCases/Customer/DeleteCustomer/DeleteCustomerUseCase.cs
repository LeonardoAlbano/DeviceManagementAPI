using DeviceManagement.Domain.Repositories;
using DeviceManagement.Exception.ExceptionsBase;

namespace DeviceManagement.Application.UseCases.Customer.DeleteCustomer;

public class DeleteCustomerUseCase : IDeleteCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomerUseCase(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            throw new NotFoundException("Customer", id);
        }

        if (customer.HasActiveDevices())
        {
            throw new ConflictException("Customer", "Cannot delete customer with active devices");
        }

        await _customerRepository.DeleteAsync(id);
        await _unitOfWork.Commit();
    }
}