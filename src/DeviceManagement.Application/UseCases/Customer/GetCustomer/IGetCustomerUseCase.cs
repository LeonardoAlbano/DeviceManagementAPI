using DeviceManagement.Communication.Responses.Customer;

namespace DeviceManagement.Application.UseCases.Customer.GetCustomer;

public interface IGetCustomerUseCase
{
    Task<CustomerResponse> Execute(Guid id);
    Task<IEnumerable<CustomerResponse>> ExecuteGetAll();
}