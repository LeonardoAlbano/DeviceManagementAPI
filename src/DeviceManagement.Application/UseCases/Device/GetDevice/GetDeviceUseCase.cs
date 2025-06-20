using DeviceManagement.Communication.Responses.Device;
using DeviceManagement.Domain.Repositories;
using DeviceManagement.Exception.ExceptionsBase;

namespace DeviceManagement.Application.UseCases.Device.GetDevice;

public class GetDeviceUseCase : IGetDeviceUseCase
{
    private readonly IDeviceRepository _deviceRepository;

    public GetDeviceUseCase(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<DeviceResponse> Execute(Guid id)
    {
        var device = await _deviceRepository.GetByIdAsync(id);
        if (device == null)
        {
            throw new NotFoundException("Device", id);
        }

        return MapToResponse(device);
    }

    public async Task<IEnumerable<DeviceResponse>> ExecuteGetAll()
    {
        var devices = await _deviceRepository.GetAllAsync();
        return devices.Select(MapToResponse);
    }

    public async Task<IEnumerable<DeviceResponse>> ExecuteGetByCustomer(Guid customerId)
    {
        var devices = await _deviceRepository.GetByCustomerIdAsync(customerId);
        return devices.Select(MapToResponse);
    }

    private static DeviceResponse MapToResponse(Domain.Entities.Device device)
    {
        return new DeviceResponse
        {
            Id = device.Id,
            Serial = device.Serial,
            IMEI = device.IMEI,
            ActivationDate = device.ActivationDate,
            CustomerId = device.CustomerId,
            Customer = device.Customer != null ? new Communication.Responses.Customer.CustomerResponse
            {
                Id = device.Customer.Id,
                Name = device.Customer.Name,
                Email = device.Customer.Email,
                Status = device.Customer.Status
            } : null,
            CreatedAt = device.CreatedAt,
            UpdatedAt = device.UpdatedAt
        };
    }
}