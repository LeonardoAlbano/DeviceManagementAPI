using DeviceManagement.Application.Services.Auth;
using DeviceManagement.Application.UseCases.Auth.Login;
using DeviceManagement.Communication.Requests.Auth;
using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Repositories;
using DeviceManagement.Exception.ExceptionsBase;
using FluentAssertions;
using Moq;
using Xunit;

namespace DeviceManagement.Application.Tests.UseCases.Auth;

public class LoginUseCaseTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly LoginUseCase _useCase;

    public LoginUseCaseTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _useCase = new LoginUseCase(_userRepositoryMock.Object, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Execute_WithValidCredentials_ShouldReturnToken()
    {
        var request = new LoginRequest
        {
            Email = "admin@test.com",
            Password = "123456"
        };

        var user = new User("Admin", "admin@test.com", "123456", "Admin");
        var token = "fake-jwt-token";

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _tokenServiceMock
            .Setup(x => x.GenerateToken(user))
            .Returns(token);

        var response = await _useCase.Execute(request);

        response.Should().NotBeNull();
        response.Token.Should().Be(token);
        response.User.Email.Should().Be(user.Email);
        response.User.Name.Should().Be(user.Name);
    }

    [Fact]
    public async Task Execute_WithNonExistentEmail_ShouldThrowValidationException()
    {
        var request = new LoginRequest
        {
            Email = "nonexistent@test.com",
            Password = "123456"
        };

        _userRepositoryMock
            .Setup(x => x.GetByEmailAsync(request.Email))
            .ReturnsAsync((User?)null);

        var act = async () => await _useCase.Execute(request);
        
        await act.Should().ThrowAsync<ValidationErrorsException>()
            .Where(ex => ex.ErrorMessages.Contains("Invalid email or password"));
    }
}
