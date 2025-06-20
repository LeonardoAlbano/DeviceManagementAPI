using DeviceManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagement.Infrastructure.DataAccess.Seed;

/// <summary>
/// Classe responsável por popular o banco com dados iniciais
/// </summary>
public static class DatabaseSeeder
{
    /// <summary>
    /// Executa o seed dos dados iniciais
    /// </summary>
    public static async Task SeedAsync(DeviceManagementDbContext context)
    {
        // Verifica se já existem dados
        if (await context.Users.AnyAsync())
        {
            return; // Dados já existem
        }

        // Cria usuário administrador padrão
        var adminUser = new User(
            name: "Administrator",
            email: "admin@devicemanagement.com",
            password: "Admin123!",
            role: "Admin"
        );

        await context.Users.AddAsync(adminUser);

        // Cria um cliente de exemplo
        var sampleCustomer = new Customer(
            name: "Empresa Exemplo Ltda",
            email: "contato@exemplo.com",
            phone: "11999999999",
            status: true
        );

        await context.Customers.AddAsync(sampleCustomer);
        await context.SaveChangesAsync();

        // Cria um dispositivo de exemplo
        var sampleDevice = new Device(
            serial: "DEV001",
            imei: "123456789012345",
            customerId: sampleCustomer.Id,
            activationDate: DateTime.UtcNow.AddDays(-30)
        );

        await context.Devices.AddAsync(sampleDevice);
        await context.SaveChangesAsync();

        // Cria alguns eventos de exemplo
        var events = new[]
        {
            new Event(Domain.Enums.EventType.TurnedOn, sampleDevice.Id, "Dispositivo ligado", DateTime.UtcNow.AddHours(-2)),
            new Event(Domain.Enums.EventType.Movement, sampleDevice.Id, "Movimento detectado", DateTime.UtcNow.AddHours(-1)),
            new Event(Domain.Enums.EventType.SignalLoss, sampleDevice.Id, "Perda de sinal", DateTime.UtcNow.AddMinutes(-30))
        };

        await context.Events.AddRangeAsync(events);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Dados iniciais criados com sucesso!");
        Console.WriteLine("📧 Email do admin: admin@devicemanagement.com");
        Console.WriteLine("🔑 Senha do admin: Admin123!");
    }
}
