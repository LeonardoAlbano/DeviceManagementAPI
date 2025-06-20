using DeviceManagement.Communication.Responses.Event;

namespace DeviceManagement.Application.UseCases.Event.GetEvent;

public interface IGetEventUseCase
{
    Task<IEnumerable<EventResponse>> ExecuteGetAll();
    Task<IEnumerable<EventResponse>> ExecuteGetByDevice(Guid deviceId);
    Task<IEnumerable<EventResponse>> ExecuteGetByPeriod(DateTime startDate, DateTime endDate);
}