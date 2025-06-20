using DeviceManagement.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace DeviceManagement.Domain.Tests.Entities;

public class DeviceTests
{
    private readonly Guid _customerId = Guid.NewGuid();

    [Fact]
    public void Device_ShouldCreateWithValidData()
    {
        var serial = "DEV001";
        var imei = "123456789012345";

        var device = new Device(serial, imei, _customerId);

        device.Serial.Should().Be(serial);
        device.IMEI.Should().Be(imei);
        device.CustomerId.Should().Be(_customerId);
        device.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Device_ShouldValidateRequiredSerial()
    {
        var act = () => new Device("", "123456789012345", _customerId);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Serial is required*");
    }

    [Fact]
    public void Device_ShouldValidateRequiredIMEI()
    {
        var act = () => new Device("DEV001", "", _customerId);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*IMEI is required*");
    }

    [Fact]
    public void Device_ShouldValidateIMEIFormat()
    {
        var act = () => new Device("DEV001", "123", _customerId);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*IMEI must have exactly 15 numeric digits*");
    }

    [Fact]
    public void Device_ShouldActivateAndDeactivate()
    {
        var device = new Device("DEV001", "123456789012345", _customerId);

        device.Activate();
        device.IsActive().Should().BeTrue();
        device.ActivationDate.Should().NotBeNull();

        device.Deactivate();
        device.IsActive().Should().BeFalse();
        device.ActivationDate.Should().BeNull();
    }
}