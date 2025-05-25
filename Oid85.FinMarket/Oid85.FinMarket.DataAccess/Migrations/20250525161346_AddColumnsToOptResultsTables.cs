using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsToOptResultsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "annual_yield_return",
                schema: "storage",
                table: "optimization_results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "total_return",
                schema: "storage",
                table: "optimization_results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "annual_yield_return",
                schema: "storage",
                table: "backtest_results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "total_return",
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
                name: "annual_yield_return",
                schema: "storage",
                table: "optimization_results");

            migrationBuilder.DropColumn(
                name: "total_return",
                schema: "storage",
                table: "optimization_results");

            migrationBuilder.DropColumn(
                name: "annual_yield_return",
                schema: "storage",
                table: "backtest_results");

            migrationBuilder.DropColumn(
                name: "total_return",
                schema: "storage",
                table: "backtest_results");
        }
    }
}
