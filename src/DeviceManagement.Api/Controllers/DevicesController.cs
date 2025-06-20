using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DeviceManagement.Application.UseCases.Device.CreateDevice;
using DeviceManagement.Application.UseCases.Device.GetDevice;
using DeviceManagement.Communication.Requests.Device;

namespace DeviceManagement.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Authorize]
public class DevicesController : ControllerBase
{
    private readonly ICreateDeviceUseCase _createDeviceUseCase;
    private readonly IGetDeviceUseCase _getDeviceUseCase;

    public DevicesController(
        ICreateDeviceUseCase createDeviceUseCase,
        IGetDeviceUseCase getDeviceUseCase)
    {
        _createDeviceUseCase = createDeviceUseCase;
        _getDeviceUseCase = getDeviceUseCase;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var response = await _getDeviceUseCase.ExecuteGetAll();
        return Ok(response);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _getDeviceUseCase.Execute(id);
        return Ok(response);
    }
    
    [HttpGet("customer/{customerId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCustomer(Guid customerId)
    {
        var response = await _getDeviceUseCase.ExecuteGetByCustomer(customerId);
        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateDeviceRequest request)
    {
        var response = await _createDeviceUseCase.Execute(request);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }
}
