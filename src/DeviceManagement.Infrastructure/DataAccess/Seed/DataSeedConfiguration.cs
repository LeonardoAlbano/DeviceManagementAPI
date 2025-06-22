using DeviceManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DeviceManagement.Infrastructure.DataAccess.Seed;

public static class DataSeedConfiguration
{
    // ✅ VALORES ESTÁTICOS - Não mudam entre builds
    private static readonly Guid AdminUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid SampleCustomerId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid SampleDeviceId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    private static readonly DateTime BaseDate = new DateTime(2024, 6, 20, 12, 0, 0, DateTimeKind.Utc);

    public static void SeedData(ModelBuilder modelBuilder)
    {
        SeedUsers(modelBuilder);
        SeedCustomers(modelBuilder);
        SeedDevices(modelBuilder);
        SeedEvents(modelBuilder);
    }

    private static void SeedUsers(ModelBuilder modelBuilder)
    {
        // ✅ Gerar hash compatível com a classe User
        var password = "Admin123@";
        var salt = "V8F+oh2ckfxVixGUeHyxtQ=="; // Salt fixo para o seed
        var passwordHash = GenerateCompatibleHash(password, salt);

        modelBuilder.Entity<User>().HasData(new
        {
            Id = AdminUserId,
            Name = "Administrator",
            Email = "admin@devicemanagement.com",
            PasswordHash = passwordHash,
            Salt = salt,
            Role = "Admin",
            IsActive = true,
            CreatedAt = BaseDate,
            UpdatedAt = BaseDate
        });
    }

    /// <summary>
    /// Gera hash compatível com o algoritmo da classe User
    /// </summary>
    private static string GenerateCompatibleHash(string password, string salt)
    {
        var combined = password + salt;
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
        return Convert.ToBase64String(hashedBytes);
    }

    private static void SeedCustomers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().HasData(new
        {
            Id = SampleCustomerId,
            Name = "Empresa Exemplo Ltda",
            Email = "contato@exemplo.com",
            Phone = "11999999999",
            Status = true,
            CreatedAt = BaseDate,
            UpdatedAt = BaseDate
        });
    }

    private static void SeedDevices(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>().HasData(new
        {
            Id = SampleDeviceId,
            Serial = "DEV001",
            IMEI = "123456789012345",
            ActivationDate = BaseDate.AddDays(-30),
            CustomerId = SampleCustomerId,
            CreatedAt = BaseDate,
            UpdatedAt = BaseDate
        });
    }

    private static void SeedEvents(ModelBuilder modelBuilder)
    {
        var events = new[]
        {
            new
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444441"),
                Type = Domain.Enums.EventType.TurnedOn,
                Observations = "Dispositivo ligado - Seed inicial",
                EventDateTime = BaseDate.AddHours(-2),
                DeviceId = SampleDeviceId,
                CreatedAt = BaseDate,
                UpdatedAt = BaseDate
            },
            new
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444442"),
                Type = Domain.Enums.EventType.Movement,
                Observations = "Movimento detectado - Seed inicial",
                EventDateTime = BaseDate.AddHours(-1),
                DeviceId = SampleDeviceId,
                CreatedAt = BaseDate,
                UpdatedAt = BaseDate
            },
            new
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444443"),
                Type = Domain.Enums.EventType.SignalLoss,
                Observations = "Perda de sinal - Seed inicial",
                EventDateTime = BaseDate.AddMinutes(-30),
                DeviceId = SampleDeviceId,
                CreatedAt = BaseDate,
                UpdatedAt = BaseDate
            }
        };

        modelBuilder.Entity<Event>().HasData(events);
    }
}
