using DeviceManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Infrastructure.DataAccess.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(DeviceManagementDbContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var adminUser = new User(
            name: "Administrator",
            email: "admin@devicemanagement.com",
            password: "Admin123@",
            role: "Admin"
        );

        await context.Users.AddAsync(adminUser);

        var sampleCustomer = new Customer(
            name: "Example Company Ltd",
            email: "contact@example.com",
            phone: "11999999999",
            status: true
        );

        await context.Customers.AddAsync(sampleCustomer);
        await context.SaveChangesAsync();

        var sampleDevice = new Device(
            serial: "DEV001",
            imei: "123456789012345",
            customerId: sampleCustomer.Id,
            activationDate: DateTime.UtcNow.AddDays(-30)
        );

        await context.Devices.AddAsync(sampleDevice);
        await context.SaveChangesAsync();

        var events = new[]
        {
            new Event(Domain.Enums.EventType.TurnedOn, sampleDevice.Id, "Device turned on", DateTime.UtcNow.AddHours(-2)),
            new Event(Domain.Enums.EventType.Movement, sampleDevice.Id, "Movement detected", DateTime.UtcNow.AddHours(-1)),
            new Event(Domain.Enums.EventType.SignalLoss, sampleDevice.Id, "Signal lost", DateTime.UtcNow.AddMinutes(-30))
        };

        await context.Events.AddRangeAsync(events);
        await context.SaveChangesAsync();
    }
}