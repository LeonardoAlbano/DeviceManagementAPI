using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeviceManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "NVARCHAR(255)", unicode: false, maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(255)", unicode: false, maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "NVARCHAR(20)", unicode: false, maxLength: 20, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    Salt = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    Role = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, defaultValue: "User"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Serial = table.Column<string>(type: "NVARCHAR(100)", unicode: false, maxLength: 100, nullable: false),
                    IMEI = table.Column<string>(type: "NVARCHAR(15)", unicode: false, maxLength: 15, nullable: false),
                    ActivationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Customers",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Type = table.Column<int>(type: "int", nullable: false, comment: "0=TurnedOn, 1=TurnedOff, 2=Movement, 3=SignalLoss"),
                    Observations = table.Column<string>(type: "NVARCHAR(500)", unicode: false, maxLength: 500, nullable: true),
                    EventDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Devices",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CreatedAt", "Email", "Name", "Phone", "Status", "UpdatedAt" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc), "contato@exemplo.com", "Empresa Exemplo Ltda", "11999999999", true, new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "Name", "PasswordHash", "Role", "Salt", "UpdatedAt" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc), "admin@devicemanagement.com", true, "Administrator", "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", "Admin", "V8F+oh2ckfxVixGUeHyxtQ==", new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "ActivationDate", "CreatedAt", "CustomerId", "IMEI", "Serial", "UpdatedAt" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2024, 5, 21, 12, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222222"), "123456789012345", "DEV001", new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "CreatedAt", "DeviceId", "EventDateTime", "Observations", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444441"), new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2024, 6, 20, 10, 0, 0, 0, DateTimeKind.Utc), "Dispositivo ligado - Seed inicial", 0, new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("44444444-4444-4444-4444-444444444442"), new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2024, 6, 20, 11, 0, 0, 0, DateTimeKind.Utc), "Movimento detectado - Seed inicial", 2, new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("44444444-4444-4444-4444-444444444443"), new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2024, 6, 20, 11, 30, 0, 0, DateTimeKind.Utc), "Perda de sinal - Seed inicial", 3, new DateTime(2024, 6, 20, 12, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Status",
                table: "Customers",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ActivationDate",
                table: "Devices",
                column: "ActivationDate");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_CustomerId",
                table: "Devices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_IMEI",
                table: "Devices",
                column: "IMEI",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_Serial",
                table: "Devices",
                column: "Serial",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_DeviceId",
                table: "Events",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_DeviceId_EventDateTime",
                table: "Events",
                columns: new[] { "DeviceId", "EventDateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventDateTime",
                table: "Events",
                column: "EventDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Events_Type",
                table: "Events",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Events_Type_EventDateTime",
                table: "Events",
                columns: new[] { "Type", "EventDateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
