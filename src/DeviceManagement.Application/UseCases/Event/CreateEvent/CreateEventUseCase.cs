using DeviceManagement.Communication.Requests.Event;
using DeviceManagement.Communication.Responses.Event;
using DeviceManagement.Domain.Repositories;
using DeviceManagement.Exception.ExceptionsBase;

namespace DeviceManagement.Application.UseCases.Event.CreateEvent;

public class CreateEventUseCase : ICreateEventUseCase
{
    private readonly IEventRepository _eventRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEventUseCase(
        IEventRepository eventRepository,
        IDeviceRepository deviceRepository,
        IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _deviceRepository = deviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EventResponse> Execute(CreateEventRequest request)
    {
        await ValidateRequest(request);

        var eventEntity = new Domain.Entities.Event(
            request.Type,
            request.DeviceId,
            request.Observations,
            request.EventDateTime
        );

        await _eventRepository.AddAsync(eventEntity);
        await _unitOfWork.Commit();
        var completeEvent = await _eventRepository.GetByIdAsync(eventEntity.Id);
        return MapToResponse(completeEvent!);
    }

    private async Task ValidateRequest(CreateEventRequest request)
    {
        var errors = new List<string>();

        if (!await _deviceRepository.ExistsAsync(request.DeviceId))
        {
            errors.Add("Device not found");
        }

        if (errors.Any())
        {
            throw new ValidationErrorsException(errors);
        }
    }

    private static EventResponse MapToResponse(Domain.Entities.Event eventEntity)
    {
        return new EventResponse
        {
            Id = eventEntity.Id,
            Type = eventEntity.Type,
            Observations = eventEntity.Observations,
            EventDateTime = eventEntity.EventDateTime,
            DeviceId = eventEntity.DeviceId,
            Device = eventEntity.Device != null ? new Communication.Responses.Device.DeviceResponse
            {
                Id = eventEntity.Device.Id,
                Serial = eventEntity.Device.Serial,
                IMEI = eventEntity.Device.IMEI,
                CustomerId = eventEntity.Device.CustomerId,
                Customer = eventEntity.Device.Customer != null ? new Communication.Responses.Customer.CustomerResponse
                {
                    Id = eventEntity.Device.Customer.Id,
                    Name = eventEntity.Device.Customer.Name,
                    Email = eventEntity.Device.Customer.Email
                } : null
            } : null,
            CreatedAt = eventEntity.CreatedAt
        };
    }
}
