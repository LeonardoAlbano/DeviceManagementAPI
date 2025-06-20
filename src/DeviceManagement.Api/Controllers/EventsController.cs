using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DeviceManagement.Application.UseCases.Event.CreateEvent;
using DeviceManagement.Application.UseCases.Event.GetEvent;
using DeviceManagement.Communication.Requests.Event;

namespace DeviceManagement.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Authorize]
public class EventsController : ControllerBase
{
    private readonly ICreateEventUseCase _createEventUseCase;
    private readonly IGetEventUseCase _getEventUseCase;

    public EventsController(
        ICreateEventUseCase createEventUseCase,
        IGetEventUseCase getEventUseCase)
    {
        _createEventUseCase = createEventUseCase;
        _getEventUseCase = getEventUseCase;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var response = await _getEventUseCase.ExecuteGetAll();
        return Ok(response);
    }
    
    [HttpGet("device/{deviceId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDevice(Guid deviceId)
    {
        var response = await _getEventUseCase.ExecuteGetByDevice(deviceId);
        return Ok(response);
    }
    
    [HttpGet("period")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var response = await _getEventUseCase.ExecuteGetByPeriod(startDate, endDate);
        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
    {
        var response = await _createEventUseCase.Execute(request);
        return CreatedAtAction("Create", new { id = response.Id }, response);
    }
}
