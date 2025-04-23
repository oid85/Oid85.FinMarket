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
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    result_string = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    result_number = table.Column<double>(type: "double precision", nullable: false),
                    analyse_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_analyse_results", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "asset_report_events",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    report_date = table.Column<DateOnly>(type: "date", nullable: false),
                    period_year = table.Column<int>(type: "integer", nullable: false),
                    period_num = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asset_report_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bond_coupons",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
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
                    last_price = table.Column<double>(type: "double precision", nullable: false),
                    isin = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    figi = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    sector = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    nkd = table.Column<double>(type: "double precision", nullable: false),
                    maturity_date = table.Column<DateOnly>(type: "date", nullable: false),
                    floating_coupon_flag = table.Column<bool>(type: "boolean", nullable: false),
                    risk_level = table.Column<int>(type: "integer", nullable: false),
                    currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
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
                name: "currencies",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    last_price = table.Column<double>(type: "double precision", nullable: false),
                    isin = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    figi = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    class_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    iso_currency_name = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                name: "daily_candles",
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
                    table.PrimaryKey("pk_daily_candles", x => x.id);
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
                name: "fear_greed_index",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    market_momentum = table.Column<double>(type: "double precision", nullable: false),
                    market_volatility = table.Column<double>(type: "double precision", nullable: false),
                    stock_price_breadth = table.Column<double>(type: "double precision", nullable: false),
                    stock_price_strength = table.Column<double>(type: "double precision", nullable: false),
                    value = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_fear_greed_index", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "fin_indexes",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    figi = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    last_price = table.Column<double>(type: "double precision", nullable: false),
                    class_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    currency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    instrument_kind = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    exchange = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                name: "five_minute_candles",
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
                    datetime = table.Column<long>(type: "bigint", nullable: false),
                    is_complete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_five_minute_candles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "forecast_consensuses",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    recommendation_string = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    recommendation_number = table.Column<int>(type: "integer", nullable: false),
                    currency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    current_price = table.Column<double>(type: "double precision", nullable: false),
                    consensus_price = table.Column<double>(type: "double precision", nullable: false),
                    min_target = table.Column<double>(type: "double precision", nullable: false),
                    max_target = table.Column<double>(type: "double precision", nullable: false),
                    price_change = table.Column<double>(type: "double precision", nullable: false),
                    price_change_rel = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_forecast_consensuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "forecast_targets",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    company = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    recommendation_string = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    recommendation_number = table.Column<int>(type: "integer", nullable: false),
                    recommendation_date = table.Column<DateOnly>(type: "date", nullable: false),
                    currency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    current_price = table.Column<double>(type: "double precision", nullable: false),
                    target_price = table.Column<double>(type: "double precision", nullable: false),
                    price_change = table.Column<double>(type: "double precision", nullable: false),
                    price_change_rel = table.Column<double>(type: "double precision", nullable: false),
                    show_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_forecast_targets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "futures",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    last_price = table.Column<double>(type: "double precision", nullable: false),
                    figi = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    expiration_date = table.Column<DateOnly>(type: "date", nullable: false),
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
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    sector = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_instruments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "market_events",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    time = table.Column<TimeOnly>(type: "time", nullable: false),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    instrument_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    market_event_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    market_event_text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    sent_notification = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_market_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "multiplicators",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker_ao = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ticker_ap = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    total_shares_ao = table.Column<double>(type: "double precision", nullable: false),
                    total_shares_ap = table.Column<double>(type: "double precision", nullable: false),
                    beta = table.Column<double>(type: "double precision", nullable: false),
                    revenue = table.Column<double>(type: "double precision", nullable: false),
                    operating_income = table.Column<double>(type: "double precision", nullable: false),
                    pe = table.Column<double>(type: "double precision", nullable: false),
                    pb = table.Column<double>(type: "double precision", nullable: false),
                    pbv = table.Column<double>(type: "double precision", nullable: false),
                    ev = table.Column<double>(type: "double precision", nullable: false),
                    bv = table.Column<double>(type: "double precision", nullable: false),
                    roe = table.Column<double>(type: "double precision", nullable: false),
                    roa = table.Column<double>(type: "double precision", nullable: false),
                    net_interest_margin = table.Column<double>(type: "double precision", nullable: false),
                    total_debt = table.Column<double>(type: "double precision", nullable: false),
                    net_debt = table.Column<double>(type: "double precision", nullable: false),
                    market_capitalization = table.Column<double>(type: "double precision", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "shares",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    last_price = table.Column<double>(type: "double precision", nullable: false),
                    isin = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    figi = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    sector = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
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
                    multiplier = table.Column<double>(type: "double precision", nullable: false),
                    price_difference = table.Column<double>(type: "double precision", nullable: false),
                    price_difference_prc = table.Column<double>(type: "double precision", nullable: false),
                    price_difference_average = table.Column<double>(type: "double precision", nullable: false),
                    price_difference_average_prc = table.Column<double>(type: "double precision", nullable: false),
                    funding = table.Column<double>(type: "double precision", nullable: false),
                    price_position = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
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
                name: "ix_asset_report_events_instrument_id",
                schema: "public",
                table: "asset_report_events",
                column: "instrument_id");

            migrationBuilder.CreateIndex(
                name: "ix_daily_candles_instrument_id",
                schema: "storage",
                table: "daily_candles",
                column: "instrument_id");

            migrationBuilder.CreateIndex(
                name: "ix_five_minute_candles_instrument_id",
                schema: "storage",
                table: "five_minute_candles",
                column: "instrument_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "analyse_results",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "asset_report_events",
                schema: "public");

            migrationBuilder.DropTable(
                name: "bond_coupons",
                schema: "public");

            migrationBuilder.DropTable(
                name: "bonds",
                schema: "public");

            migrationBuilder.DropTable(
                name: "currencies",
                schema: "public");

            migrationBuilder.DropTable(
                name: "daily_candles",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "dividend_infos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "fear_greed_index",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "fin_indexes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "five_minute_candles",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "forecast_consensuses",
                schema: "public");

            migrationBuilder.DropTable(
                name: "forecast_targets",
                schema: "public");

            migrationBuilder.DropTable(
                name: "futures",
                schema: "public");

            migrationBuilder.DropTable(
                name: "instruments",
                schema: "public");

            migrationBuilder.DropTable(
                name: "market_events",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "multiplicators",
                schema: "public");

            migrationBuilder.DropTable(
                name: "shares",
                schema: "public");

            migrationBuilder.DropTable(
                name: "spreads",
                schema: "storage");
        }
    }
}
