using DeviceManagement.Communication.Responses.Event;
using DeviceManagement.Domain.Repositories;

namespace DeviceManagement.Application.UseCases.Event.GetEvent;

public class GetEventUseCase : IGetEventUseCase
{
    private readonly IEventRepository _eventRepository;

    public GetEventUseCase(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<IEnumerable<EventResponse>> ExecuteGetAll()
    {
        var events = await _eventRepository.GetAllAsync();
        return events.Select(MapToResponse);
    }

    public async Task<IEnumerable<EventResponse>> ExecuteGetByDevice(Guid deviceId)
    {
        var events = await _eventRepository.GetByDeviceIdAsync(deviceId);
        return events.Select(MapToResponse);
    }

    public async Task<IEnumerable<EventResponse>> ExecuteGetByPeriod(DateTime startDate, DateTime endDate)
    {
        var events = await _eventRepository.GetByPeriodAsync(startDate, endDate);
        return events.Select(MapToResponse);
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
