using DeviceManagement.Communication.Requests.Customer;
using DeviceManagement.Communication.Responses.Customer;
using DeviceManagement.Domain.Repositories;
using DeviceManagement.Exception.ExceptionsBase;

namespace DeviceManagement.Application.UseCases.Customer.UpdateCustomer;

public class UpdateCustomerUseCase : IUpdateCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerUseCase(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerResponse> Execute(Guid id, UpdateCustomerRequest request)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            throw new NotFoundException("Customer", id);
        }

        await ValidateRequest(request, id);

        customer.UpdateData(request.Name, request.Email, request.Phone);
        
        if (request.Status != customer.Status)
        {
            if (request.Status)
                customer.Activate();
            else
                customer.Deactivate();
        }

        await _customerRepository.UpdateAsync(customer);
        await _unitOfWork.Commit();

        return MapToResponse(customer);
    }

    private async Task ValidateRequest(UpdateCustomerRequest request, Guid customerId)
    {
        var errors = new List<string>();

        if (await _customerRepository.EmailExistsAsync(request.Email, customerId))
        {
            errors.Add("Email is already in use by another customer");
        }

        if (errors.Any())
        {
            throw new ValidationErrorsException(errors);
        }
    }

    private static CustomerResponse MapToResponse(Domain.Entities.Customer customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            Status = customer.Status,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt,
            TotalDevices = customer.TotalDevices
        };
    }
}
