using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DeviceManagement.Application.UseCases.Customer.CreateCustomer;
using DeviceManagement.Application.UseCases.Customer.GetCustomer;
using DeviceManagement.Application.UseCases.Customer.UpdateCustomer;
using DeviceManagement.Application.UseCases.Customer.DeleteCustomer;
using DeviceManagement.Communication.Requests.Customer;

namespace DeviceManagement.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ICreateCustomerUseCase _createCustomerUseCase;
    private readonly IGetCustomerUseCase _getCustomerUseCase;
    private readonly IUpdateCustomerUseCase _updateCustomerUseCase;
    private readonly IDeleteCustomerUseCase _deleteCustomerUseCase;

    public CustomersController(
        ICreateCustomerUseCase createCustomerUseCase,
        IGetCustomerUseCase getCustomerUseCase,
        IUpdateCustomerUseCase updateCustomerUseCase,
        IDeleteCustomerUseCase deleteCustomerUseCase)
    {
        _createCustomerUseCase = createCustomerUseCase;
        _getCustomerUseCase = getCustomerUseCase;
        _updateCustomerUseCase = updateCustomerUseCase;
        _deleteCustomerUseCase = deleteCustomerUseCase;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var response = await _getCustomerUseCase.ExecuteGetAll();
        return Ok(response);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _getCustomerUseCase.Execute(id);
        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
    {
        var response = await _createCustomerUseCase.Execute(request);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerRequest request)
    {
        var response = await _updateCustomerUseCase.Execute(id, request);
        return Ok(response);
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _deleteCustomerUseCase.Execute(id);
        return NoContent();
    }
}
