using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ParkingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParkingSpots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Number = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    IsOccupied = table.Column<bool>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingSpots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LicensePlate = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Model = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    EntryTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExitTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ParkingSpotId = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsParked = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_ParkingSpots_ParkingSpotId",
                        column: x => x.ParkingSpotId,
                        principalTable: "ParkingSpots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ParkingSpots",
                columns: new[] { "Id", "IsOccupied", "Number", "Type" },
                values: new object[,]
                {
                    { 1, false, "A01", 1 },
                    { 2, false, "A02", 1 },
                    { 3, false, "A03", 1 },
                    { 4, false, "A04", 1 },
                    { 5, false, "A05", 1 },
                    { 6, false, "A06", 1 },
                    { 7, false, "A07", 1 },
                    { 8, false, "A08", 1 },
                    { 9, false, "A09", 1 },
                    { 10, false, "A10", 1 },
                    { 11, false, "A11", 1 },
                    { 12, false, "A12", 1 },
                    { 13, false, "A13", 1 },
                    { 14, false, "A14", 1 },
                    { 15, false, "A15", 1 },
                    { 16, false, "A16", 1 },
                    { 17, false, "A17", 1 },
                    { 18, false, "A18", 1 },
                    { 19, false, "A19", 1 },
                    { 20, false, "A20", 1 },
                    { 21, false, "A21", 1 },
                    { 22, false, "A22", 1 },
                    { 23, false, "A23", 1 },
                    { 24, false, "A24", 1 },
                    { 25, false, "A25", 1 },
                    { 26, false, "A26", 1 },
                    { 27, false, "A27", 1 },
                    { 28, false, "A28", 1 },
                    { 29, false, "A29", 1 },
                    { 30, false, "A30", 1 },
                    { 31, false, "A31", 1 },
                    { 32, false, "A32", 1 },
                    { 33, false, "A33", 1 },
                    { 34, false, "A34", 1 },
                    { 35, false, "A35", 1 },
                    { 36, false, "A36", 1 },
                    { 37, false, "A37", 1 },
                    { 38, false, "A38", 1 },
                    { 39, false, "A39", 1 },
                    { 40, false, "A40", 1 },
                    { 41, false, "A41", 2 },
                    { 42, false, "A42", 2 },
                    { 43, false, "A43", 2 },
                    { 44, false, "A44", 2 },
                    { 45, false, "A45", 2 },
                    { 46, false, "A46", 3 },
                    { 47, false, "A47", 3 },
                    { 48, false, "A48", 4 },
                    { 49, false, "A49", 4 },
                    { 50, false, "A50", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpots_Number",
                table: "ParkingSpots",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_LicensePlate",
                table: "Vehicles",
                column: "LicensePlate");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ParkingSpotId",
                table: "Vehicles",
                column: "ParkingSpotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "ParkingSpots");
        }
    }
}
