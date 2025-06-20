using DeviceManagement.Domain.Entities;

namespace DeviceManagement.Application.Services.Auth;

public interface ITokenService
{
    string GenerateToken(User user);
    bool ValidateToken(string token);
    Guid? GetUserIdFromToken(string token);
}