using DeviceManagement.Domain.Entities;
using DeviceManagement.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace DeviceManagement.Domain.Tests.Entities;

public class EventTests
{
    private readonly Guid _deviceId = Guid.NewGuid();

    [Fact]
    public void Event_ShouldCreateWithValidData()
    {
        var type = EventType.TurnedOn;
        var observations = "Device turned on successfully";

        var eventEntity = new Event(type, _deviceId, observations);

        eventEntity.Type.Should().Be(type);
        eventEntity.DeviceId.Should().Be(_deviceId);
        eventEntity.Observations.Should().Be(observations);
        eventEntity.EventDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Event_ShouldValidateRequiredDeviceId()
    {
        var act = () => new Event(EventType.TurnedOn, Guid.Empty);
        act.Should().Throw<ArgumentException>()
            .WithMessage("*DeviceId is required*");
    }

    [Fact]
    public void Event_ShouldIdentifyCriticalEvent()
    {
        var normalEvent = new Event(EventType.TurnedOn, _deviceId);
        var criticalEvent = new Event(EventType.SignalLoss, _deviceId);

        normalEvent.IsCritical().Should().BeFalse();
        criticalEvent.IsCritical().Should().BeTrue();
    }

    [Fact]
    public void Event_ShouldGetTypeDescription()
    {
        var eventEntity = new Event(EventType.Movement, _deviceId);

        var description = eventEntity.GetTypeDescription();

        description.Should().Be("Movement Detected");
    }
}