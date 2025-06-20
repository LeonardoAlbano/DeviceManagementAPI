using DeviceManagement.Communication.Requests.Device;
using DeviceManagement.Communication.Responses.Device;

namespace DeviceManagement.Application.UseCases.Device.CreateDevice;

public interface ICreateDeviceUseCase
{
    Task<DeviceResponse> Execute(CreateDeviceRequest request);
}