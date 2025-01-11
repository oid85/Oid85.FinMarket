using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTableMultiplicators : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_candles",
                schema: "storage",
                table: "candles");

            migrationBuilder.DropColumn(
                name: "in_watch_list",
                schema: "storage",
                table: "spreads");

            migrationBuilder.RenameTable(
                name: "candles",
                schema: "storage",
                newName: "daily-candles",
                newSchema: "storage");

            migrationBuilder.RenameIndex(
                name: "ix_candles_instrument_id",
                schema: "storage",
                table: "daily-candles",
                newName: "ix_daily_candles_instrument_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_daily_candles",
                schema: "storage",
                table: "daily-candles",
                column: "id");

            migrationBuilder.CreateTable(
                name: "five-minute-candles",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    open = table.Column<double>(type: "double precision", nullable: false),
                    close = table.Column<double>(type: "double precision", nullable: false),
                    high = table.Column<double>(type: "double precision", nullable: false),
                    low = table.Column<double>(type: "double precision", nullable: false),
                    volume = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    time = table.Column<TimeOnly>(type: "time", nullable: false),
                    is_complete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_five_minute_candles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "multiplicators",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    market_capitalization = table.Column<double>(type: "double precision", nullable: false),
                    low_of_year = table.Column<double>(type: "double precision", nullable: false),
                    high_of_year = table.Column<double>(type: "double precision", nullable: false),
                    beta = table.Column<double>(type: "double precision", nullable: false),
                    net_income = table.Column<double>(type: "double precision", nullable: false),
                    ebitda = table.Column<double>(type: "double precision", nullable: false),
                    eps = table.Column<double>(type: "double precision", nullable: false),
                    free_cash_flow = table.Column<double>(type: "double precision", nullable: false),
                    ev_to_ebitda = table.Column<double>(type: "double precision", nullable: false),
                    total_debt_to_ebitda = table.Column<double>(type: "double precision", nullable: false),
                    net_debt_to_ebitda = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_multiplicators", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_five_minute_candles_instrument_id",
                schema: "storage",
                table: "five-minute-candles",
                column: "instrument_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "five-minute-candles",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "multiplicators",
                schema: "public");

            migrationBuilder.DropPrimaryKey(
                name: "pk_daily_candles",
                schema: "storage",
                table: "daily-candles");

            migrationBuilder.RenameTable(
                name: "daily-candles",
                schema: "storage",
                newName: "candles",
                newSchema: "storage");

            migrationBuilder.RenameIndex(
                name: "ix_daily_candles_instrument_id",
                schema: "storage",
                table: "candles",
                newName: "ix_candles_instrument_id");

            migrationBuilder.AddColumn<bool>(
                name: "in_watch_list",
                schema: "storage",
                table: "spreads",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "pk_candles",
                schema: "storage",
                table: "candles",
                column: "id");
        }
    }
}
