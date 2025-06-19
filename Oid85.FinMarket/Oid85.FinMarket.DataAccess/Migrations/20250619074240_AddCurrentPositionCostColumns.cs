using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrentPositionCostColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "current_position_cost",
                schema: "storage",
                table: "optimization_results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "current_position_cost",
                schema: "storage",
                table: "backtest_results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "current_position_cost",
                schema: "storage",
                table: "optimization_results");

            migrationBuilder.DropColumn(
                name: "current_position_cost",
                schema: "storage",
                table: "backtest_results");
        }
    }
}
