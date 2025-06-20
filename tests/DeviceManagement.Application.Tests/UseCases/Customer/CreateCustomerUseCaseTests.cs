using DeviceManagement.Application.UseCases.Customer.CreateCustomer;
using DeviceManagement.Communication.Requests.Customer;
using DeviceManagement.Domain.Repositories;
using DeviceManagement.Exception.ExceptionsBase;
using FluentAssertions;
using Moq;
using Xunit;

namespace DeviceManagement.Application.Tests.UseCases.Customer;

public class CreateCustomerUseCaseTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateCustomerUseCase _useCase;

    public CreateCustomerUseCaseTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _useCase = new CreateCustomerUseCase(_customerRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Execute_WithValidData_ShouldCreateCustomer()
    {
        var request = new CreateCustomerRequest
        {
            Name = "John Smith",
            Email = "john@email.com",
            Phone = "11999999999",
            Status = true
        };

        _customerRepositoryMock
            .Setup(x => x.EmailExistsAsync(request.Email, null))
            .ReturnsAsync(false);

        var response = await _useCase.Execute(request);

        response.Should().NotBeNull();
        response.Name.Should().Be(request.Name);
        response.Email.Should().Be(request.Email);
        response.Phone.Should().Be(request.Phone);
        response.Status.Should().Be(request.Status);

        _customerRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Customer>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
    }

    [Fact]
    public async Task Execute_WithExistingEmail_ShouldThrowValidationException()
    {
        var request = new CreateCustomerRequest
        {
            Name = "John Smith",
            Email = "john@email.com"
        };

        _customerRepositoryMock
            .Setup(x => x.EmailExistsAsync(request.Email, null))
            .ReturnsAsync(true);

        var act = async () => await _useCase.Execute(request);
        
        await act.Should().ThrowAsync<ValidationErrorsException>()
            .Where(ex => ex.ErrorMessages.Contains("Email is already in use by another customer"));
    }
}
