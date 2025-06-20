using Microsoft.AspNetCore.Mvc;
using DeviceManagement.Application.UseCases.Dashboard.GetDashboard;

namespace DeviceManagement.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class DashboardController : ControllerBase
{
    private readonly IGetDashboardUseCase _getDashboardUseCase;

    public DashboardController(IGetDashboardUseCase getDashboardUseCase)
    {
        _getDashboardUseCase = getDashboardUseCase;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDashboard()
    {
        var response = await _getDashboardUseCase.Execute();
        return Ok(response);
    }
}