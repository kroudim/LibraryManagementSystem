using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reservation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BookId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerPartyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BorrowedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReturnedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "BookId", "BorrowedAt", "CustomerPartyId", "IsActive", "ReturnedAt" },
                values: new object[] { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2026, 2, 3, 14, 13, 5, 381, DateTimeKind.Utc).AddTicks(7366), new Guid("44444444-4444-4444-4444-444444444444"), true, null });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CustomerPartyId_BookId_IsActive",
                table: "Reservations",
                columns: new[] { "CustomerPartyId", "BookId", "IsActive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");
        }
    }
}
