using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditOptBacktestResultTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "streak_lost_longest",
                schema: "storage",
                table: "optimization_results");

            migrationBuilder.DropColumn(
                name: "streak_won_longest",
                schema: "storage",
                table: "optimization_results");

            migrationBuilder.DropColumn(
                name: "streak_lost_longest",
                schema: "storage",
                table: "backtest_results");

            migrationBuilder.DropColumn(
                name: "streak_won_longest",
                schema: "storage",
                table: "backtest_results");

            migrationBuilder.RenameColumn(
                name: "total_open",
                schema: "storage",
                table: "optimization_results",
                newName: "winning_positions");

            migrationBuilder.RenameColumn(
                name: "total_closed",
                schema: "storage",
                table: "optimization_results",
                newName: "number_positions");

            migrationBuilder.RenameColumn(
                name: "total",
                schema: "storage",
                table: "optimization_results",
                newName: "current_position");

            migrationBuilder.RenameColumn(
                name: "sharp_ratio",
                schema: "storage",
                table: "optimization_results",
                newName: "winning_trades_percent");

            migrationBuilder.RenameColumn(
                name: "pnl_net_total",
                schema: "storage",
                table: "optimization_results",
                newName: "start_money");

            migrationBuilder.RenameColumn(
                name: "pnl_net_average",
                schema: "storage",
                table: "optimization_results",
                newName: "net_profit");

            migrationBuilder.RenameColumn(
                name: "total_open",
                schema: "storage",
                table: "backtest_results",
                newName: "winning_positions");

            migrationBuilder.RenameColumn(
                name: "total_closed",
                schema: "storage",
                table: "backtest_results",
                newName: "number_positions");

            migrationBuilder.RenameColumn(
                name: "total",
                schema: "storage",
                table: "backtest_results",
                newName: "current_position");

            migrationBuilder.RenameColumn(
                name: "sharp_ratio",
                schema: "storage",
                table: "backtest_results",
                newName: "winning_trades_percent");

            migrationBuilder.RenameColumn(
                name: "pnl_net_total",
                schema: "storage",
                table: "backtest_results",
                newName: "start_money");

            migrationBuilder.RenameColumn(
                name: "pnl_net_average",
                schema: "storage",
                table: "backtest_results",
                newName: "net_profit");

            migrationBuilder.AddColumn<double>(
                name: "average_profit",
                schema: "storage",
                table: "optimization_results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "average_profit_percent",
                schema: "storage",
                table: "optimization_results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "end_date",
                schema: "storage",
                table: "optimization_results",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<double>(
                name: "end_money",
                schema: "storage",
                table: "optimization_results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "max_drawdown",
                schema: "storage",
                table: "optimization_results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "start_date",
                schema: "storage",
                table: "optimization_results",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<double>(
                name: "average_profit",
                schema: "storage",
                table: "backtest_results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "average_profit_percent",
                schema: "storage",
                table: "backtest_results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "end_date",
                schema: "storage",
                table: "backtest_results",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<double>(
                name: "end_money",
                schema: "storage",
                table: "backtest_results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "max_drawdown",
                schema: "storage",
                table: "backtest_results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "start_date",
                schema: "storage",
                table: "backtest_results",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "average_profit",
                schema: "storage",
                table: "optimization_results");

            migrationBuilder.DropColumn(
                name: "average_profit_percent",
                schema: "storage",
                table: "optimization_results");

            migrationBuilder.DropColumn(
                name: "end_date",
                schema: "storage",
                table: "optimization_results");

            migrationBuilder.DropColumn(
                name: "end_money",
                schema: "storage",
                table: "optimization_results");

            migrationBuilder.DropColumn(
                name: "max_drawdown",
                schema: "storage",
                table: "optimization_results");

            migrationBuilder.DropColumn(
                name: "start_date",
                schema: "storage",
                table: "optimization_results");

            migrationBuilder.DropColumn(
                name: "average_profit",
                schema: "storage",
                table: "backtest_results");

            migrationBuilder.DropColumn(
                name: "average_profit_percent",
                schema: "storage",
                table: "backtest_results");

            migrationBuilder.DropColumn(
                name: "end_date",
                schema: "storage",
                table: "backtest_results");

            migrationBuilder.DropColumn(
                name: "end_money",
                schema: "storage",
                table: "backtest_results");

            migrationBuilder.DropColumn(
                name: "max_drawdown",
                schema: "storage",
                table: "backtest_results");

            migrationBuilder.DropColumn(
                name: "start_date",
                schema: "storage",
                table: "backtest_results");

            migrationBuilder.RenameColumn(
                name: "winning_trades_percent",
                schema: "storage",
                table: "optimization_results",
                newName: "sharp_ratio");

            migrationBuilder.RenameColumn(
                name: "winning_positions",
                schema: "storage",
                table: "optimization_results",
                newName: "total_open");

            migrationBuilder.RenameColumn(
                name: "start_money",
                schema: "storage",
                table: "optimization_results",
                newName: "pnl_net_total");

            migrationBuilder.RenameColumn(
                name: "number_positions",
                schema: "storage",
                table: "optimization_results",
                newName: "total_closed");

            migrationBuilder.RenameColumn(
                name: "net_profit",
                schema: "storage",
                table: "optimization_results",
                newName: "pnl_net_average");

            migrationBuilder.RenameColumn(
                name: "current_position",
                schema: "storage",
                table: "optimization_results",
                newName: "total");

            migrationBuilder.RenameColumn(
                name: "winning_trades_percent",
                schema: "storage",
                table: "backtest_results",
                newName: "sharp_ratio");

            migrationBuilder.RenameColumn(
                name: "winning_positions",
                schema: "storage",
                table: "backtest_results",
                newName: "total_open");

            migrationBuilder.RenameColumn(
                name: "start_money",
                schema: "storage",
                table: "backtest_results",
                newName: "pnl_net_total");

            migrationBuilder.RenameColumn(
                name: "number_positions",
                schema: "storage",
                table: "backtest_results",
                newName: "total_closed");

            migrationBuilder.RenameColumn(
                name: "net_profit",
                schema: "storage",
                table: "backtest_results",
                newName: "pnl_net_average");

            migrationBuilder.RenameColumn(
                name: "current_position",
                schema: "storage",
                table: "backtest_results",
                newName: "total");

            migrationBuilder.AddColumn<int>(
                name: "streak_lost_longest",
                schema: "storage",
                table: "optimization_results",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "streak_won_longest",
                schema: "storage",
                table: "optimization_results",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "streak_lost_longest",
                schema: "storage",
                table: "backtest_results",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "streak_won_longest",
                schema: "storage",
                table: "backtest_results",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
