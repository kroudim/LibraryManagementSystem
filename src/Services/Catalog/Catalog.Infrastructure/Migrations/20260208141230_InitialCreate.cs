using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ISBN = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AuthorPartyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalCopies = table.Column<int>(type: "integer", nullable: false),
                    AvailableCopies = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Fiction" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "Mystery" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorPartyId", "AvailableCopies", "CategoryId", "CreatedAt", "ISBN", "Title", "TotalCopies", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("88888888-8888-8888-8888-888888888888"), new Guid("33333333-3333-3333-3333-333333333333"), 5, new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2026, 2, 8, 14, 12, 29, 248, DateTimeKind.Utc).AddTicks(8870), "978-1234567890", "The Great Adventure", 5, new DateTime(2026, 2, 8, 14, 12, 29, 248, DateTimeKind.Utc).AddTicks(9144) },
                    { new Guid("99999999-9999-9999-9999-999999999999"), new Guid("33333333-3333-3333-3333-333333333333"), 3, new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2026, 2, 8, 14, 12, 29, 248, DateTimeKind.Utc).AddTicks(9405), "978-0987654321", "Mystery of the Lost City", 3, new DateTime(2026, 2, 8, 14, 12, 29, 248, DateTimeKind.Utc).AddTicks(9406) },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("33333333-3333-3333-3333-333333333333"), 10, new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2026, 2, 8, 14, 12, 29, 248, DateTimeKind.Utc).AddTicks(9410), "978-1122334455", "Science Fiction Tales", 10, new DateTime(2026, 2, 8, 14, 12, 29, 248, DateTimeKind.Utc).AddTicks(9410) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ISBN",
                table: "Books",
                column: "ISBN",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
