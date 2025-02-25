using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAssetFundamentalTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "asset_fundamentals",
                schema: "public");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "asset_fundamentals",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    adr_to_common_share_ratio = table.Column<double>(type: "double precision", nullable: false),
                    average_daily_volume_last_10_days = table.Column<double>(type: "double precision", nullable: false),
                    average_daily_volume_last_4_weeks = table.Column<double>(type: "double precision", nullable: false),
                    beta = table.Column<double>(type: "double precision", nullable: false),
                    buy_back_ttm = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    currency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    current_ratio_mrq = table.Column<double>(type: "double precision", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    diluted_eps_ttm = table.Column<double>(type: "double precision", nullable: false),
                    dividend_payout_ratio_fy = table.Column<double>(type: "double precision", nullable: false),
                    dividend_rate_ttm = table.Column<double>(type: "double precision", nullable: false),
                    dividend_yield_daily_ttm = table.Column<double>(type: "double precision", nullable: false),
                    dividends_per_share = table.Column<double>(type: "double precision", nullable: false),
                    domicile_indicator_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ebitda_change_five_years = table.Column<double>(type: "double precision", nullable: false),
                    ebitda_ttm = table.Column<double>(type: "double precision", nullable: false),
                    eps_change_five_years = table.Column<double>(type: "double precision", nullable: false),
                    eps_ttm = table.Column<double>(type: "double precision", nullable: false),
                    ev_to_ebitda_mrq = table.Column<double>(type: "double precision", nullable: false),
                    ev_to_sales = table.Column<double>(type: "double precision", nullable: false),
                    ex_dividend_date = table.Column<DateOnly>(type: "date", nullable: false),
                    fiscal_period_end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    fiscal_period_start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    five_year_annual_dividend_growth_rate = table.Column<double>(type: "double precision", nullable: false),
                    five_year_annual_revenue_growth_rate = table.Column<double>(type: "double precision", nullable: false),
                    five_years_average_dividend_yield = table.Column<double>(type: "double precision", nullable: false),
                    fixed_charge_coverage_ratio_fy = table.Column<double>(type: "double precision", nullable: false),
                    forward_annual_dividend_yield = table.Column<double>(type: "double precision", nullable: false),
                    free_cash_flow_to_price = table.Column<double>(type: "double precision", nullable: false),
                    free_cash_flow_ttm = table.Column<double>(type: "double precision", nullable: false),
                    free_float = table.Column<double>(type: "double precision", nullable: false),
                    high_price_last_52_weeks = table.Column<double>(type: "double precision", nullable: false),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    low_price_last_52_weeks = table.Column<double>(type: "double precision", nullable: false),
                    market_capitalization = table.Column<double>(type: "double precision", nullable: false),
                    net_debt_to_ebitda = table.Column<double>(type: "double precision", nullable: false),
                    net_income_ttm = table.Column<double>(type: "double precision", nullable: false),
                    net_interest_margin_mrq = table.Column<double>(type: "double precision", nullable: false),
                    net_margin_mrq = table.Column<double>(type: "double precision", nullable: false),
                    number_of_employees = table.Column<double>(type: "double precision", nullable: false),
                    one_year_annual_revenue_growth_rate = table.Column<double>(type: "double precision", nullable: false),
                    pe_ratio_ttm = table.Column<double>(type: "double precision", nullable: false),
                    price_to_book_ttm = table.Column<double>(type: "double precision", nullable: false),
                    price_to_free_cash_flow_ttm = table.Column<double>(type: "double precision", nullable: false),
                    price_to_sales_ttm = table.Column<double>(type: "double precision", nullable: false),
                    revenue_change_five_years = table.Column<double>(type: "double precision", nullable: false),
                    revenue_ttm = table.Column<double>(type: "double precision", nullable: false),
                    roa = table.Column<double>(type: "double precision", nullable: false),
                    roe = table.Column<double>(type: "double precision", nullable: false),
                    roic = table.Column<double>(type: "double precision", nullable: false),
                    shares_outstanding = table.Column<double>(type: "double precision", nullable: false),
                    three_year_annual_revenue_growth_rate = table.Column<double>(type: "double precision", nullable: false),
                    total_debt_change_five_years = table.Column<double>(type: "double precision", nullable: false),
                    total_debt_mrq = table.Column<double>(type: "double precision", nullable: false),
                    total_debt_to_ebitda_mrq = table.Column<double>(type: "double precision", nullable: false),
                    total_debt_to_equity_mrq = table.Column<double>(type: "double precision", nullable: false),
                    total_enterprise_value_mrq = table.Column<double>(type: "double precision", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asset_fundamentals", x => x.id);
                });
        }
    }
}
