using DeviceManagement.Communication.Responses.Device;

namespace DeviceManagement.Application.UseCases.Device.GetDevice;

public interface IGetDeviceUseCase
{
    Task<DeviceResponse> Execute(Guid id);
    Task<IEnumerable<DeviceResponse>> ExecuteGetAll();
    Task<IEnumerable<DeviceResponse>> ExecuteGetByCustomer(Guid customerId);
}