using DeviceManagement.Communication.Requests.Device;
using DeviceManagement.Communication.Responses.Device;
using DeviceManagement.Domain.Repositories;
using DeviceManagement.Exception.ExceptionsBase;

namespace DeviceManagement.Application.UseCases.Device.CreateDevice;

public class CreateDeviceUseCase : ICreateDeviceUseCase
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDeviceUseCase(
        IDeviceRepository deviceRepository,
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _deviceRepository = deviceRepository;
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DeviceResponse> Execute(CreateDeviceRequest request)
    {
        await ValidateRequest(request);

        var device = new Domain.Entities.Device(
            request.Serial,
            request.IMEI,
            request.CustomerId,
            request.ActivationDate
        );

        await _deviceRepository.AddAsync(device);
        await _unitOfWork.Commit();

        var completeDevice = await _deviceRepository.GetByIdAsync(device.Id);
        return MapToResponse(completeDevice!);
    }

    private async Task ValidateRequest(CreateDeviceRequest request)
    {
        var errors = new List<string>();

        if (!await _customerRepository.ExistsAsync(request.CustomerId))
        {
            errors.Add("Customer not found");
        }

        if (await _deviceRepository.SerialExistsAsync(request.Serial))
        {
            errors.Add("Serial is already in use");
        }

        if (await _deviceRepository.IMEIExistsAsync(request.IMEI))
        {
            errors.Add("IMEI is already in use");
        }

        if (errors.Any())
        {
            throw new ValidationErrorsException(errors);
        }
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
