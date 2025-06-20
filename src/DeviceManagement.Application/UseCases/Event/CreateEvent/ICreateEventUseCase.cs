using DeviceManagement.Communication.Requests.Event;
using DeviceManagement.Communication.Responses.Event;

namespace DeviceManagement.Application.UseCases.Event.CreateEvent;

public interface ICreateEventUseCase
{
    Task<EventResponse> Execute(CreateEventRequest request);
}