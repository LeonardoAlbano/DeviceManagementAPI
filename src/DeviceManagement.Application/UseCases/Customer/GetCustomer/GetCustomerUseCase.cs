using DeviceManagement.Communication.Responses.Customer;
using DeviceManagement.Domain.Repositories;
using DeviceManagement.Exception.ExceptionsBase;

namespace DeviceManagement.Application.UseCases.Customer.GetCustomer;

public class GetCustomerUseCase : IGetCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerResponse> Execute(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer == null)
        {
            throw new NotFoundException("Customer not found");
        }

        return new CustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            Status = customer.Status,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        };
    }

    public async Task<IEnumerable<CustomerResponse>> ExecuteGetAll()
    {
        var customers = await _customerRepository.GetAllAsync();

        return customers.Select(customer => new CustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            Status = customer.Status,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt
        });
    }
}