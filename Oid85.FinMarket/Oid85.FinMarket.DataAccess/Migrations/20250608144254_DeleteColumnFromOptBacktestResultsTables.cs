using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DeleteColumnFromOptBacktestResultsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "strategy_version",
                schema: "storage",
                table: "optimization_results");

            migrationBuilder.DropColumn(
                name: "strategy_version",
                schema: "storage",
                table: "backtest_results");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "strategy_version",
                schema: "storage",
                table: "optimization_results",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "strategy_version",
                schema: "storage",
                table: "backtest_results",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
