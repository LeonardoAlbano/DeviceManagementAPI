using DeviceManagement.Communication.Requests.Auth;
using DeviceManagement.Communication.Responses.Auth;

namespace DeviceManagement.Application.UseCases.Auth.Login;

public interface ILoginUseCase
{
    Task<LoginResponse> Execute(LoginRequest request);
}