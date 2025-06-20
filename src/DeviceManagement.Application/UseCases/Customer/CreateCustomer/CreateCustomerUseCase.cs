using DeviceManagement.Communication.Requests.Customer;
using DeviceManagement.Communication.Responses.Customer;
using DeviceManagement.Domain.Repositories;
using DeviceManagement.Exception.ExceptionsBase;

namespace DeviceManagement.Application.UseCases.Customer.CreateCustomer;

public class CreateCustomerUseCase : ICreateCustomerUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerUseCase(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerResponse> Execute(CreateCustomerRequest request)
    {
        await ValidateRequest(request);

        var customer = new Domain.Entities.Customer(
            request.Name,
            request.Email,
            request.Phone,
            request.Status
        );

        await _customerRepository.AddAsync(customer);
        await _unitOfWork.Commit();

        return MapToResponse(customer);
    }

    private async Task ValidateRequest(CreateCustomerRequest request)
    {
        var errors = new List<string>();

        if (await _customerRepository.EmailExistsAsync(request.Email))
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