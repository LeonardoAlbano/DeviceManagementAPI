using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeviceManagement.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "Email", "Name", "Phone", "Status", "UpdatedAt" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc), "contact@example.com", "Example Company Ltd.", "11999999999", true, new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "Name", "PasswordHash", "Role", "Salt", "UpdatedAt" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc), "admin@devicemanagement.com", true, "Administrator", "4AaJ9AbWA1GYMhUSgRFxX6Npy7ig1OEcFvsRcy8YBkk=", "Admin", "V8F+oh2ckfxVixGUeHyxtQ==", new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "ActivationDate", "CreatedAt", "CustomerId", "IMEI", "Serial", "UpdatedAt" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2024, 5, 21, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222222"), "123456789012345", "DEV001", new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "CreatedAt", "DeviceId", "EventDateTime", "Observations", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444441"), new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2024, 6, 20, 10, 0, 0, 0, DateTimeKind.Utc), "Device turned on - Initial seed", 0, new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("44444444-4444-4444-4444-444444444442"), new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2024, 6, 20, 11, 0, 0, 0, DateTimeKind.Utc), "Movement detected - Initial seed", 2, new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("44444444-4444-4444-4444-444444444443"), new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2024, 6, 20, 11, 30, 0, 0, DateTimeKind.Utc), "Signal loss - Initial seed", 3, new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444441"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444442"));

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444443"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Devices",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));
        }
    }
}
