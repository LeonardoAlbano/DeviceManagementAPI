using DeviceManagement.Application.Services.Auth;
using DeviceManagement.Communication.Requests.Auth;
using DeviceManagement.Communication.Responses.Auth;
using DeviceManagement.Domain.Repositories;
using DeviceManagement.Exception.ExceptionsBase;

namespace DeviceManagement.Application.UseCases.Auth.Login;

public class LoginUseCase : ILoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public LoginUseCase(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Execute(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        
        if (user == null || !user.VerifyPassword(request.Password))
        {
            throw new ValidationErrorsException(new List<string> { "Invalid email or password" });
        }

        if (!user.IsActive)
        {
            throw new ValidationErrorsException(new List<string> { "Inactive user" });
        }

        var token = _tokenService.GenerateToken(user);

        return new LoginResponse
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(8),
            User = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive
            }
        };
    }
}