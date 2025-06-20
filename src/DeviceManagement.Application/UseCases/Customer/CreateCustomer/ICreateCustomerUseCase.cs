using DeviceManagement.Communication.Requests.Customer;
using DeviceManagement.Communication.Responses.Customer;

namespace DeviceManagement.Application.UseCases.Customer.CreateCustomer;

public interface ICreateCustomerUseCase
{
    Task<CustomerResponse> Execute(CreateCustomerRequest request);
}