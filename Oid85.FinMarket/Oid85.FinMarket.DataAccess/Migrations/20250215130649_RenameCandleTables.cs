using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameCandleTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "five-minute-candles",
                schema: "storage",
                newName: "five_minute_candles",
                newSchema: "storage");

            migrationBuilder.RenameTable(
                name: "daily-candles",
                schema: "storage",
                newName: "daily_candles",
                newSchema: "storage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "five_minute_candles",
                schema: "storage",
                newName: "five-minute-candles",
                newSchema: "storage");

            migrationBuilder.RenameTable(
                name: "daily_candles",
                schema: "storage",
                newName: "daily-candles",
                newSchema: "storage");
        }
    }
}
