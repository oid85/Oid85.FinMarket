using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class _25082025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "statistical_arbitrage_backtest_results",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    ticker_first = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ticker_second = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    timeframe = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    strategy_id = table.Column<Guid>(type: "uuid", nullable: false),
                    strategy_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    strategy_name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    strategy_params = table.Column<string>(type: "jsonb", maxLength: 1000, nullable: false),
                    strategy_params_hash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    number_positions = table.Column<int>(type: "integer", nullable: false),
                    current_position_first = table.Column<int>(type: "integer", nullable: false),
                    current_position_second = table.Column<int>(type: "integer", nullable: false),
                    current_position_cost = table.Column<double>(type: "double precision", nullable: false),
                    profit_factor = table.Column<double>(type: "double precision", nullable: false),
                    recovery_factor = table.Column<double>(type: "double precision", nullable: false),
                    net_profit = table.Column<double>(type: "double precision", nullable: false),
                    average_profit = table.Column<double>(type: "double precision", nullable: false),
                    average_profit_percent = table.Column<double>(type: "double precision", nullable: false),
                    drawdown = table.Column<double>(type: "double precision", nullable: false),
                    max_drawdown = table.Column<double>(type: "double precision", nullable: false),
                    max_drawdown_percent = table.Column<double>(type: "double precision", nullable: false),
                    winning_positions = table.Column<int>(type: "integer", nullable: false),
                    winning_trades_percent = table.Column<double>(type: "double precision", nullable: false),
                    start_money = table.Column<double>(type: "double precision", nullable: false),
                    end_money = table.Column<double>(type: "double precision", nullable: false),
                    total_return = table.Column<double>(type: "double precision", nullable: false),
                    annual_yield_return = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statistical_arbitrage_backtest_results", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "statistical_arbitrage_optimization_results",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    ticker_first = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ticker_second = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    timeframe = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    strategy_id = table.Column<Guid>(type: "uuid", nullable: false),
                    strategy_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    strategy_name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    strategy_params = table.Column<string>(type: "jsonb", maxLength: 1000, nullable: false),
                    strategy_params_hash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    number_positions = table.Column<int>(type: "integer", nullable: false),
                    current_position_first = table.Column<int>(type: "integer", nullable: false),
                    current_position_second = table.Column<int>(type: "integer", nullable: false),
                    current_position_cost = table.Column<double>(type: "double precision", nullable: false),
                    profit_factor = table.Column<double>(type: "double precision", nullable: false),
                    recovery_factor = table.Column<double>(type: "double precision", nullable: false),
                    net_profit = table.Column<double>(type: "double precision", nullable: false),
                    average_profit = table.Column<double>(type: "double precision", nullable: false),
                    average_profit_percent = table.Column<double>(type: "double precision", nullable: false),
                    drawdown = table.Column<double>(type: "double precision", nullable: false),
                    max_drawdown = table.Column<double>(type: "double precision", nullable: false),
                    max_drawdown_percent = table.Column<double>(type: "double precision", nullable: false),
                    winning_positions = table.Column<int>(type: "integer", nullable: false),
                    winning_trades_percent = table.Column<double>(type: "double precision", nullable: false),
                    start_money = table.Column<double>(type: "double precision", nullable: false),
                    end_money = table.Column<double>(type: "double precision", nullable: false),
                    total_return = table.Column<double>(type: "double precision", nullable: false),
                    annual_yield_return = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statistical_arbitrage_optimization_results", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "statistical_arbitrage_strategy_signals",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker_first = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ticker_second = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    count_signals = table.Column<int>(type: "integer", nullable: false),
                    count_strategies = table.Column<int>(type: "integer", nullable: false),
                    percent_signals = table.Column<double>(type: "double precision", nullable: false),
                    position_cost = table.Column<double>(type: "double precision", nullable: false),
                    position_size_first = table.Column<int>(type: "integer", nullable: false),
                    position_size_second = table.Column<int>(type: "integer", nullable: false),
                    position_percent_portfolio = table.Column<double>(type: "double precision", nullable: false),
                    last_price_first = table.Column<double>(type: "double precision", nullable: false),
                    last_price_second = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statistical_arbitrage_strategy_signals", x => x.id);
                });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3862), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3863) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3913), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3914) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3855), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3855) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3917), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3917) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3860), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3860) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3857), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3858) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3868), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3868) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3775), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3777) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3852), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3853) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3865), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3865) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3846), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3846) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3850), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3850) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "statistical_arbitrage_backtest_results",
                schema: "public");

            migrationBuilder.DropTable(
                name: "statistical_arbitrage_optimization_results",
                schema: "public");

            migrationBuilder.DropTable(
                name: "statistical_arbitrage_strategy_signals",
                schema: "public");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9743), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9743) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9751), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9751) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9735), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9735) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9753), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9753) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9741) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9738), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9738) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9748), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9748) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9616), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9621) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9700), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9701) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9746), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9746) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9694), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9694) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9698), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9698) });
        }
    }
}
