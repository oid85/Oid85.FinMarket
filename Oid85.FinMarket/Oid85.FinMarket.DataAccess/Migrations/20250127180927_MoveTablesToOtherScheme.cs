using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MoveTablesToOtherScheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "market_events",
                schema: "dictionary",
                newName: "market_events",
                newSchema: "storage");

            migrationBuilder.RenameTable(
                name: "instruments",
                schema: "dictionary",
                newName: "instruments",
                newSchema: "public");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dictionary");

            migrationBuilder.RenameTable(
                name: "market_events",
                schema: "storage",
                newName: "market_events",
                newSchema: "dictionary");

            migrationBuilder.RenameTable(
                name: "instruments",
                schema: "public",
                newName: "instruments",
                newSchema: "dictionary");
        }
    }
}
