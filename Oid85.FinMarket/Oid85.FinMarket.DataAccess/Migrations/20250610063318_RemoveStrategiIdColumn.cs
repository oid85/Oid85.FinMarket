using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStrategiIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "strategy_id",
                schema: "storage",
                table: "optimization_results");

            migrationBuilder.DropColumn(
                name: "strategy_id",
                schema: "storage",
                table: "backtest_results");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "strategy_id",
                schema: "storage",
                table: "optimization_results",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "strategy_id",
                schema: "storage",
                table: "backtest_results",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");
        }
    }
}
