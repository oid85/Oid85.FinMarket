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

            migrationBuilder.EnsureSchema(
                name: "dictionary");

            migrationBuilder.CreateTable(
                name: "analyse_results",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    result = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    analyse_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_analyse_results", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "asset_fundamentals",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: false),
                    market_capitalization = table.Column<double>(type: "double precision", nullable: false),
                    high_price_last_52_weeks = table.Column<double>(type: "double precision", nullable: false),
                    low_price_last_52_weeks = table.Column<double>(type: "double precision", nullable: false),
                    average_daily_volume_last_10_days = table.Column<double>(type: "double precision", nullable: false),
                    average_daily_volume_last_4_weeks = table.Column<double>(type: "double precision", nullable: false),
                    beta = table.Column<double>(type: "double precision", nullable: false),
                    free_float = table.Column<double>(type: "double precision", nullable: false),
                    forward_annual_dividend_yield = table.Column<double>(type: "double precision", nullable: false),
                    shares_outstanding = table.Column<double>(type: "double precision", nullable: false),
                    revenue_ttm = table.Column<double>(type: "double precision", nullable: false),
                    ebitda_ttm = table.Column<double>(type: "double precision", nullable: false),
                    net_income_ttm = table.Column<double>(type: "double precision", nullable: false),
                    eps_ttm = table.Column<double>(type: "double precision", nullable: false),
                    diluted_eps_ttm = table.Column<double>(type: "double precision", nullable: false),
                    free_cash_flow_ttm = table.Column<double>(type: "double precision", nullable: false),
                    five_year_annual_revenue_growth_rate = table.Column<double>(type: "double precision", nullable: false),
                    three_year_annual_revenue_growth_rate = table.Column<double>(type: "double precision", nullable: false),
                    pe_ratio_ttm = table.Column<double>(type: "double precision", nullable: false),
                    price_to_sales_ttm = table.Column<double>(type: "double precision", nullable: false),
                    price_to_book_ttm = table.Column<double>(type: "double precision", nullable: false),
                    price_to_free_cash_flow_ttm = table.Column<double>(type: "double precision", nullable: false),
                    total_enterprise_value_mrq = table.Column<double>(type: "double precision", nullable: false),
                    ev_to_ebitda_mrq = table.Column<double>(type: "double precision", nullable: false),
                    net_margin_mrq = table.Column<double>(type: "double precision", nullable: false),
                    net_interest_margin_mrq = table.Column<double>(type: "double precision", nullable: false),
                    roe = table.Column<double>(type: "double precision", nullable: false),
                    roa = table.Column<double>(type: "double precision", nullable: false),
                    roic = table.Column<double>(type: "double precision", nullable: false),
                    total_debt_mrq = table.Column<double>(type: "double precision", nullable: false),
                    total_debt_to_equity_mrq = table.Column<double>(type: "double precision", nullable: false),
                    total_debt_to_ebitda_mrq = table.Column<double>(type: "double precision", nullable: false),
                    free_cash_flow_to_price = table.Column<double>(type: "double precision", nullable: false),
                    net_debt_to_ebitda = table.Column<double>(type: "double precision", nullable: false),
                    current_ratio_mrq = table.Column<double>(type: "double precision", nullable: false),
                    fixed_charge_coverage_ratio_fy = table.Column<double>(type: "double precision", nullable: false),
                    dividend_yield_daily_ttm = table.Column<double>(type: "double precision", nullable: false),
                    dividend_rate_ttm = table.Column<double>(type: "double precision", nullable: false),
                    dividends_per_share = table.Column<double>(type: "double precision", nullable: false),
                    five_years_average_dividend_yield = table.Column<double>(type: "double precision", nullable: false),
                    five_year_annual_dividend_growth_rate = table.Column<double>(type: "double precision", nullable: false),
                    dividend_payout_ratio_fy = table.Column<double>(type: "double precision", nullable: false),
                    buy_back_ttm = table.Column<double>(type: "double precision", nullable: false),
                    one_year_annual_revenue_growth_rate = table.Column<double>(type: "double precision", nullable: false),
                    domicile_indicator_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    adr_to_common_share_ratio = table.Column<double>(type: "double precision", nullable: false),
                    number_of_employees = table.Column<double>(type: "double precision", nullable: false),
                    ex_dividend_date = table.Column<DateOnly>(type: "date", nullable: false),
                    fiscal_period_start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    fiscal_period_end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    revenue_change_five_years = table.Column<double>(type: "double precision", nullable: false),
                    eps_change_five_years = table.Column<double>(type: "double precision", nullable: false),
                    ebitda_change_five_years = table.Column<double>(type: "double precision", nullable: false),
                    total_debt_change_five_years = table.Column<double>(type: "double precision", nullable: false),
                    ev_to_sales = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asset_fundamentals", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bond_coupons",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
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
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    isin = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    figi = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    sector = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
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
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    open = table.Column<double>(type: "double precision", nullable: false),
                    close = table.Column<double>(type: "double precision", nullable: false),
                    high = table.Column<double>(type: "double precision", nullable: false),
                    low = table.Column<double>(type: "double precision", nullable: false),
                    volume = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
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
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    isin = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    figi = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    class_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    iso_currency_name = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
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
                name: "fin_indexes",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    figi = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    class_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    currency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    instrument_kind = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    exchange = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    in_watch_list = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_fin_indexes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "futures",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    figi = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    expiration_date = table.Column<DateOnly>(type: "date", nullable: false),
                    in_watch_list = table.Column<bool>(type: "boolean", nullable: false),
                    lot = table.Column<int>(type: "integer", nullable: false),
                    first_trade_date = table.Column<DateOnly>(type: "date", nullable: false),
                    last_tradedate = table.Column<DateOnly>(name: "last_trade-date", type: "date", nullable: false),
                    future_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    asset_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    basic_asset = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    basic_asset_size = table.Column<double>(type: "double precision", nullable: false),
                    initial_margin_on_buy = table.Column<double>(type: "double precision", nullable: false),
                    initial_margin_on_sell = table.Column<double>(type: "double precision", nullable: false),
                    min_price_increment_amount = table.Column<double>(type: "double precision", nullable: false),
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
                name: "instruments",
                schema: "dictionary",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_instruments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shares",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    isin = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    figi = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    sector = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "spreads",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    datetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    first_instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_instrument_ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    first_instrument_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    first_instrument_price = table.Column<double>(type: "double precision", nullable: false),
                    second_instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    second_instrument_ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    second_instrument_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    second_instrument_price = table.Column<double>(type: "double precision", nullable: false),
                    price_difference = table.Column<double>(type: "double precision", nullable: false),
                    price_difference_prc = table.Column<double>(type: "double precision", nullable: false),
                    funding = table.Column<double>(type: "double precision", nullable: false),
                    price_position = table.Column<int>(type: "integer", nullable: false),
                    in_watch_list = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_spreads", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_analyse_results_instrument_id",
                schema: "storage",
                table: "analyse_results",
                column: "instrument_id");

            migrationBuilder.CreateIndex(
                name: "ix_candles_instrument_id",
                schema: "storage",
                table: "candles",
                column: "instrument_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "analyse_results",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "asset_fundamentals",
                schema: "public");

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
                name: "fin_indexes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "futures",
                schema: "public");

            migrationBuilder.DropTable(
                name: "instruments",
                schema: "dictionary");

            migrationBuilder.DropTable(
                name: "shares",
                schema: "public");

            migrationBuilder.DropTable(
                name: "spreads",
                schema: "storage");
        }
    }
}
