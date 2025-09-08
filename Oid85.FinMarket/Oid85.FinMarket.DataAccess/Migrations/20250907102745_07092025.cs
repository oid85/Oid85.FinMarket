using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class _07092025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "pair_arbitrage_backtest_results",
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
                    table.PrimaryKey("PK_pair_arbitrage_backtest_results", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pair_arbitrage_optimization_results",
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
                    table.PrimaryKey("PK_pair_arbitrage_optimization_results", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pair_arbitrage_strategy_signals",
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
                    table.PrimaryKey("PK_pair_arbitrage_strategy_signals", x => x.id);
                });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8768), new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8768) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8776), new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8776) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8760), new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8760) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8778), new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8778) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8765), new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8765) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8763), new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8763) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8773), new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8773) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8654), new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8656) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8757), new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8757) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8770), new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8770) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8750), new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8750) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8754), new DateTime(2025, 9, 7, 10, 27, 44, 937, DateTimeKind.Utc).AddTicks(8754) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pair_arbitrage_backtest_results",
                schema: "public");

            migrationBuilder.DropTable(
                name: "pair_arbitrage_optimization_results",
                schema: "public");

            migrationBuilder.DropTable(
                name: "pair_arbitrage_strategy_signals",
                schema: "public");

            migrationBuilder.CreateTable(
                name: "statistical_arbitrage_backtest_results",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    annual_yield_return = table.Column<double>(type: "double precision", nullable: false),
                    average_profit = table.Column<double>(type: "double precision", nullable: false),
                    average_profit_percent = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    current_position_cost = table.Column<double>(type: "double precision", nullable: false),
                    current_position_first = table.Column<int>(type: "integer", nullable: false),
                    current_position_second = table.Column<int>(type: "integer", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    drawdown = table.Column<double>(type: "double precision", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_money = table.Column<double>(type: "double precision", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    max_drawdown = table.Column<double>(type: "double precision", nullable: false),
                    max_drawdown_percent = table.Column<double>(type: "double precision", nullable: false),
                    net_profit = table.Column<double>(type: "double precision", nullable: false),
                    number_positions = table.Column<int>(type: "integer", nullable: false),
                    profit_factor = table.Column<double>(type: "double precision", nullable: false),
                    recovery_factor = table.Column<double>(type: "double precision", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_money = table.Column<double>(type: "double precision", nullable: false),
                    strategy_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    strategy_id = table.Column<Guid>(type: "uuid", nullable: false),
                    strategy_name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    strategy_params = table.Column<string>(type: "jsonb", maxLength: 1000, nullable: false),
                    strategy_params_hash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ticker_first = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ticker_second = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    timeframe = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    total_return = table.Column<double>(type: "double precision", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    winning_positions = table.Column<int>(type: "integer", nullable: false),
                    winning_trades_percent = table.Column<double>(type: "double precision", nullable: false)
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
                    annual_yield_return = table.Column<double>(type: "double precision", nullable: false),
                    average_profit = table.Column<double>(type: "double precision", nullable: false),
                    average_profit_percent = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    current_position_cost = table.Column<double>(type: "double precision", nullable: false),
                    current_position_first = table.Column<int>(type: "integer", nullable: false),
                    current_position_second = table.Column<int>(type: "integer", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    drawdown = table.Column<double>(type: "double precision", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_money = table.Column<double>(type: "double precision", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    max_drawdown = table.Column<double>(type: "double precision", nullable: false),
                    max_drawdown_percent = table.Column<double>(type: "double precision", nullable: false),
                    net_profit = table.Column<double>(type: "double precision", nullable: false),
                    number_positions = table.Column<int>(type: "integer", nullable: false),
                    profit_factor = table.Column<double>(type: "double precision", nullable: false),
                    recovery_factor = table.Column<double>(type: "double precision", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_money = table.Column<double>(type: "double precision", nullable: false),
                    strategy_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    strategy_id = table.Column<Guid>(type: "uuid", nullable: false),
                    strategy_name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    strategy_params = table.Column<string>(type: "jsonb", maxLength: 1000, nullable: false),
                    strategy_params_hash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ticker_first = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ticker_second = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    timeframe = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    total_return = table.Column<double>(type: "double precision", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    winning_positions = table.Column<int>(type: "integer", nullable: false),
                    winning_trades_percent = table.Column<double>(type: "double precision", nullable: false)
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
                    count_signals = table.Column<int>(type: "integer", nullable: false),
                    count_strategies = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    last_price_first = table.Column<double>(type: "double precision", nullable: false),
                    last_price_second = table.Column<double>(type: "double precision", nullable: false),
                    percent_signals = table.Column<double>(type: "double precision", nullable: false),
                    position_cost = table.Column<double>(type: "double precision", nullable: false),
                    position_percent_portfolio = table.Column<double>(type: "double precision", nullable: false),
                    position_size_first = table.Column<int>(type: "integer", nullable: false),
                    position_size_second = table.Column<int>(type: "integer", nullable: false),
                    ticker_first = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ticker_second = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(98), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(98) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(107), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(107) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(87), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(87) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(109), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(109) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(96), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(96) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(90), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(90) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(104), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(104) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 250, DateTimeKind.Utc).AddTicks(9990), new DateTime(2025, 8, 29, 16, 15, 47, 250, DateTimeKind.Utc).AddTicks(9992) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(59), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(59) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(102), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(102) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(53), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(53) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(56), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(56) });
        }
    }
}
