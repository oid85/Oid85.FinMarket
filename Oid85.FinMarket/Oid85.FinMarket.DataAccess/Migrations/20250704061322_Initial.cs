using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

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
                    table.PrimaryKey("PK_analyse_results", x => x.id);
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
                    table.PrimaryKey("PK_asset_report_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "backtest_results",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    timeframe = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    strategy_id = table.Column<Guid>(type: "uuid", nullable: false),
                    strategy_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    strategy_name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    strategy_params = table.Column<string>(type: "jsonb", maxLength: 1000, nullable: false),
                    strategy_params_hash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    number_positions = table.Column<int>(type: "integer", nullable: false),
                    current_position = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_backtest_results", x => x.id);
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
                    table.PrimaryKey("PK_bond_coupons", x => x.id);
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
                    table.PrimaryKey("PK_bonds", x => x.id);
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
                    table.PrimaryKey("PK_currencies", x => x.id);
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
                    table.PrimaryKey("PK_daily_candles", x => x.id);
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
                    table.PrimaryKey("PK_dividend_infos", x => x.id);
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
                    table.PrimaryKey("PK_fear_greed_index", x => x.id);
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
                    table.PrimaryKey("PK_fin_indexes", x => x.id);
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
                    table.PrimaryKey("PK_forecast_consensuses", x => x.id);
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
                    table.PrimaryKey("PK_forecast_targets", x => x.id);
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
                    table.PrimaryKey("PK_futures", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hourly_candles",
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
                    table.PrimaryKey("PK_hourly_candles", x => x.id);
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
                    table.PrimaryKey("PK_instruments", x => x.id);
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
                    table.PrimaryKey("PK_market_events", x => x.id);
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
                    table.PrimaryKey("PK_multiplicators", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "optimization_results",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    timeframe = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    strategy_id = table.Column<Guid>(type: "uuid", nullable: false),
                    strategy_description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    strategy_name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    strategy_params = table.Column<string>(type: "jsonb", maxLength: 1000, nullable: false),
                    strategy_params_hash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    number_positions = table.Column<int>(type: "integer", nullable: false),
                    current_position = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_optimization_results", x => x.id);
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
                    table.PrimaryKey("PK_shares", x => x.id);
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
                    table.PrimaryKey("PK_spreads", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "strategy_signals",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    count_signals = table.Column<int>(type: "integer", nullable: false),
                    position_cost = table.Column<double>(type: "double precision", nullable: false),
                    position_size = table.Column<int>(type: "integer", nullable: false),
                    last_price = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_strategy_signals", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "instruments",
                columns: new[] { "id", "instrument_id", "name", "sector", "ticker", "type" },
                values: new object[,]
                {
                    { new Guid("05804fbf-35a1-481c-bbf2-4acfc3996da3"), new Guid("6503b433-233c-4d99-bea6-017e7dcad033"), "Oil and Gas Sector Index", "", "OGSI", "Index" },
                    { new Guid("271bb1d1-4476-4638-bae8-0cac6f7179ad"), new Guid("8a9ca1af-e4fb-4138-9695-0fccec865480"), "Finance Sector Index", "", "FSI", "Index" },
                    { new Guid("3a20bcb8-6be5-4510-b91a-f3096918686c"), new Guid("8a923a18-1dda-46a8-b163-15491d62314a"), "Banks Sector Index", "", "BSI", "Index" },
                    { new Guid("712e7169-5953-46a0-829e-400b9015a56d"), new Guid("42ef3680-1ea0-42e3-b587-a0abe1a23dc7"), "IT Sector Index", "", "ITSI", "Index" },
                    { new Guid("97094d1f-8426-44fc-a25d-a556ec9c3a97"), new Guid("da5ae557-fb6c-4cc2-886d-4bc4aedce12a"), "Non Ferrous Metallurgy Sector Index", "", "NFMSI", "Index" },
                    { new Guid("abe702e9-0271-4622-9f52-4de2da88ebfc"), new Guid("299bc8a4-db16-4fef-8adb-f9cfe138f0eb"), "Energ Sector Index", "", "ESI", "Index" },
                    { new Guid("c3db74d9-048b-4fe7-9abe-3d67a1b4010f"), new Guid("2faf88b4-037f-40fc-ade1-5b72452d9c15"), "Mining Sector Index", "", "MSI", "Index" },
                    { new Guid("d05f0a65-9d0c-42dc-8dd1-130ba5bfeb3e"), new Guid("b916084e-dcee-440f-ac44-6240913753f6"), "Housing And Utilities Sector Index", "", "HUSI", "Index" },
                    { new Guid("d2615eb5-c224-4e7d-9bb7-8bfa4e1351ea"), new Guid("8b18fbbf-5d4e-4578-8586-1a104ad1dcb1"), "IronAndSteelIndustry Sector Index", "", "ISISI", "Index" },
                    { new Guid("d9edc8e2-df33-484d-b509-74b55c44396d"), new Guid("f039936b-5183-46d1-a262-feff6b83a377"), "Telecom Sector Index", "", "TSI", "Index" },
                    { new Guid("e0683c7c-68b0-4d9a-a3b4-8f94086df49f"), new Guid("6d35ac04-9c84-4c4d-b2fa-7aae12b2fcdd"), "Retail Sector Index", "", "RSI", "Index" },
                    { new Guid("e5d093cd-e00d-4603-9c81-c33aa0a1bbc6"), new Guid("b0d7ac17-6042-48ab-ad79-076bf950e451"), "Transport Sector Index", "", "TRSI", "Index" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_analyse_results_instrument_id",
                schema: "storage",
                table: "analyse_results",
                column: "instrument_id");

            migrationBuilder.CreateIndex(
                name: "IX_asset_report_events_instrument_id",
                schema: "public",
                table: "asset_report_events",
                column: "instrument_id");

            migrationBuilder.CreateIndex(
                name: "IX_daily_candles_instrument_id",
                schema: "storage",
                table: "daily_candles",
                column: "instrument_id");

            migrationBuilder.CreateIndex(
                name: "IX_hourly_candles_instrument_id",
                schema: "storage",
                table: "hourly_candles",
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
                name: "backtest_results",
                schema: "storage");

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
                name: "forecast_consensuses",
                schema: "public");

            migrationBuilder.DropTable(
                name: "forecast_targets",
                schema: "public");

            migrationBuilder.DropTable(
                name: "futures",
                schema: "public");

            migrationBuilder.DropTable(
                name: "hourly_candles",
                schema: "storage");

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
                name: "optimization_results",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "shares",
                schema: "public");

            migrationBuilder.DropTable(
                name: "spreads",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "strategy_signals",
                schema: "storage");
        }
    }
}
