using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "storage");

            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "analyse_results",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "text", nullable: false),
                    timeframe = table.Column<string>(type: "text", nullable: false),
                    result = table.Column<string>(type: "text", nullable: false),
                    analyse_type = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_analyse_results", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bond_coupons",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    coupon_date = table.Column<DateOnly>(type: "date", nullable: false),
                    coupon_number = table.Column<long>(type: "bigint", nullable: false),
                    coupon_period = table.Column<int>(type: "integer", nullable: false),
                    coupon_start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    coupon_end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    pay_one_bond = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bond_coupons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bonds",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    isin = table.Column<string>(type: "text", nullable: false),
                    figi = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    sector = table.Column<string>(type: "text", nullable: false),
                    in_portfolio = table.Column<bool>(type: "boolean", nullable: false),
                    in_watch_list = table.Column<bool>(type: "boolean", nullable: false),
                    nkd = table.Column<double>(type: "double precision", nullable: false),
                    maturity_date = table.Column<DateOnly>(type: "date", nullable: false),
                    floating_coupon_flag = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bonds", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "candles",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "text", nullable: false),
                    timeframe = table.Column<string>(type: "text", nullable: false),
                    open = table.Column<double>(type: "double precision", nullable: false),
                    close = table.Column<double>(type: "double precision", nullable: false),
                    high = table.Column<double>(type: "double precision", nullable: false),
                    low = table.Column<double>(type: "double precision", nullable: false),
                    volume = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_complete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_candles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "currencies",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    isin = table.Column<string>(type: "text", nullable: false),
                    figi = table.Column<string>(type: "text", nullable: false),
                    class_code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    iso_currency_name = table.Column<string>(type: "text", nullable: false),
                    uid = table.Column<string>(type: "text", nullable: false),
                    in_watch_list = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_currencies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dividend_infos",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "text", nullable: false),
                    record_date = table.Column<DateOnly>(type: "date", nullable: false),
                    declared_date = table.Column<DateOnly>(type: "date", nullable: false),
                    dividend = table.Column<double>(type: "double precision", nullable: false),
                    dividend_prc = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dividend_infos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "futures",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    figi = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    in_portfolio = table.Column<bool>(type: "boolean", nullable: false),
                    in_watch_list = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_futures", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "indicatives",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    figi = table.Column<string>(type: "text", nullable: false),
                    ticker = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    class_code = table.Column<string>(type: "text", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: false),
                    instrument_kind = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    exchange = table.Column<string>(type: "text", nullable: false),
                    uid = table.Column<string>(type: "text", nullable: false),
                    in_watch_list = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_indicatives", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shares",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    isin = table.Column<string>(type: "text", nullable: false),
                    figi = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    sector = table.Column<string>(type: "text", nullable: false),
                    in_irus_index = table.Column<bool>(type: "boolean", nullable: false),
                    in_portfolio = table.Column<bool>(type: "boolean", nullable: false),
                    in_watch_list = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shares", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_analyse_results_ticker",
                schema: "storage",
                table: "analyse_results",
                column: "ticker");

            migrationBuilder.CreateIndex(
                name: "ix_candles_ticker",
                schema: "storage",
                table: "candles",
                column: "ticker");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "analyse_results",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "bond_coupons",
                schema: "public");

            migrationBuilder.DropTable(
                name: "bonds",
                schema: "public");

            migrationBuilder.DropTable(
                name: "candles",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "currencies",
                schema: "public");

            migrationBuilder.DropTable(
                name: "dividend_infos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "futures",
                schema: "public");

            migrationBuilder.DropTable(
                name: "indicatives",
                schema: "public");

            migrationBuilder.DropTable(
                name: "shares",
                schema: "public");
        }
    }
}
