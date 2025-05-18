using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTablesForOptimizationAndBacktesting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "backtest_results",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    timeframe = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    strategy_id = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    strategy_version = table.Column<int>(type: "integer", nullable: false),
                    strategy_params = table.Column<string>(type: "jsonb", maxLength: 1000, nullable: false),
                    strategy_params_hash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    total = table.Column<int>(type: "integer", nullable: false),
                    total_open = table.Column<int>(type: "integer", nullable: false),
                    total_closed = table.Column<int>(type: "integer", nullable: false),
                    streak_won_longest = table.Column<int>(type: "integer", nullable: false),
                    streak_lost_longest = table.Column<int>(type: "integer", nullable: false),
                    pnl_net_total = table.Column<double>(type: "double precision", nullable: false),
                    pnl_net_average = table.Column<double>(type: "double precision", nullable: false),
                    max_drawdown_percent = table.Column<double>(type: "double precision", nullable: false),
                    profit_factor = table.Column<double>(type: "double precision", nullable: false),
                    recovery_factor = table.Column<double>(type: "double precision", nullable: false),
                    sharp_ratio = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_backtest_results", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "optimization_results",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    timeframe = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    strategy_id = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    strategy_version = table.Column<int>(type: "integer", nullable: false),
                    strategy_params = table.Column<string>(type: "jsonb", maxLength: 1000, nullable: false),
                    strategy_params_hash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    total = table.Column<int>(type: "integer", nullable: false),
                    total_open = table.Column<int>(type: "integer", nullable: false),
                    total_closed = table.Column<int>(type: "integer", nullable: false),
                    streak_won_longest = table.Column<int>(type: "integer", nullable: false),
                    streak_lost_longest = table.Column<int>(type: "integer", nullable: false),
                    pnl_net_total = table.Column<double>(type: "double precision", nullable: false),
                    pnl_net_average = table.Column<double>(type: "double precision", nullable: false),
                    max_drawdown_percent = table.Column<double>(type: "double precision", nullable: false),
                    profit_factor = table.Column<double>(type: "double precision", nullable: false),
                    recovery_factor = table.Column<double>(type: "double precision", nullable: false),
                    sharp_ratio = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_optimization_results", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "strategy_signals",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    position = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_strategy_signals", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "backtest_results",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "optimization_results",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "strategy_signals",
                schema: "storage");
        }
    }
}
