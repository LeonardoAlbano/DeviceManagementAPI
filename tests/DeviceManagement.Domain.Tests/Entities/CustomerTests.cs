using DeviceManagement.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace DeviceManagement.Domain.Tests.Entities;

public class CustomerTests
{
    [Fact]
    public void Customer_ShouldCreateWithValidData()
    {
        var name = "John Smith";
        var email = "john@email.com";
        var phone = "11999999999";

        var customer = new Customer(name, email, phone);

        customer.Name.Should().Be(name);
        customer.Email.Should().Be(email);
        customer.Phone.Should().Be(phone);
        customer.Status.Should().BeTrue();
        customer.Id.Should().NotBe(Guid.Empty);
        customer.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        customer.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Customer_ShouldValidateRequiredName()
    {
        var act = () => new Customer("", "john@email.com", "11999999999");
        act.Should().Throw<ArgumentException>()
           .WithMessage("*Name is required*");
    }

    [Fact]
    public void Customer_ShouldValidateRequiredEmail()
    {
        var act = () => new Customer("John", "", "11999999999");
        act.Should().Throw<ArgumentException>()
           .WithMessage("*Email is required*");
    }

    [Fact]
    public void Customer_ShouldValidateValidEmail()
    {
        var act = () => new Customer("John", "invalid-email", "11999999999");
        act.Should().Throw<ArgumentException>()
           .WithMessage("*Invalid email*");
    }

    [Fact]
    public void Customer_ShouldActivateAndDeactivate()
    {
        var customer = new Customer("John", "john@email.com");

        customer.Deactivate();
        customer.Status.Should().BeFalse();

        customer.Activate();
        customer.Status.Should().BeTrue();
    }

    [Fact]
    public void Customer_ShouldUpdateData()
    {
        var customer = new Customer("John", "john@email.com");
        var newName = "John Smith";
        var newEmail = "john.smith@email.com";
        var newPhone = "11888888888";

        customer.UpdateData(newName, newEmail, newPhone);

        customer.Name.Should().Be(newName);
        customer.Email.Should().Be(newEmail);
        customer.Phone.Should().Be(newPhone);
    }
}
