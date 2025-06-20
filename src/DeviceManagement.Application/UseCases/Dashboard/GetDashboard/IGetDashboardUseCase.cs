using DeviceManagement.Communication.Responses.Dashboard;

namespace DeviceManagement.Application.UseCases.Dashboard.GetDashboard;

public interface IGetDashboardUseCase
{
    Task<DashboardResponse> Execute();
}