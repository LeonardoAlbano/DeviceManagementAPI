namespace DeviceManagement.Application.UseCases.Customer.DeleteCustomer;

public interface IDeleteCustomerUseCase
{
    Task Execute(Guid id);
}