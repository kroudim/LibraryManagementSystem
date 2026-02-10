using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Party.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartyRoles",
                columns: table => new
                {
                    PartyId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyRoles", x => new { x.PartyId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_PartyRoles_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Parties",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 2, 8, 14, 11, 54, 419, DateTimeKind.Utc).AddTicks(156), "john.author@library.com", "John", "Author", new DateTime(2026, 2, 8, 14, 11, 54, 419, DateTimeKind.Utc).AddTicks(466) },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 2, 8, 14, 11, 54, 419, DateTimeKind.Utc).AddTicks(816), "jane.customer@library.com", "Jane", "Customer", new DateTime(2026, 2, 8, 14, 11, 54, 419, DateTimeKind.Utc).AddTicks(817) },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 2, 8, 14, 11, 54, 419, DateTimeKind.Utc).AddTicks(819), "bob.both@library.com", "Bob", "Both", new DateTime(2026, 2, 8, 14, 11, 54, 419, DateTimeKind.Utc).AddTicks(820) }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Author" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Customer" }
                });

            migrationBuilder.InsertData(
                table: "PartyRoles",
                columns: new[] { "PartyId", "RoleId", "AssignedAt" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 2, 8, 14, 11, 54, 419, DateTimeKind.Utc).AddTicks(2376) },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 2, 8, 14, 11, 54, 419, DateTimeKind.Utc).AddTicks(2679) },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 2, 8, 14, 11, 54, 419, DateTimeKind.Utc).AddTicks(2682) },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 2, 8, 14, 11, 54, 419, DateTimeKind.Utc).AddTicks(2683) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parties_Email",
                table: "Parties",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyRoles_RoleId",
                table: "PartyRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartyRoles");

            migrationBuilder.DropTable(
                name: "Parties");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
