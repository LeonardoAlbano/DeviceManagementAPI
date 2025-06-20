using DeviceManagement.Communication.Requests.Customer;
using DeviceManagement.Communication.Responses.Customer;

namespace DeviceManagement.Application.UseCases.Customer.UpdateCustomer;

public interface IUpdateCustomerUseCase
{
    Task<CustomerResponse> Execute(Guid id, UpdateCustomerRequest request);
}