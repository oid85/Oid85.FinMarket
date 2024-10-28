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

            migrationBuilder.EnsureSchema(
                name: "dict");

            migrationBuilder.CreateTable(
                name: "bonds",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "text", nullable: false),
                    isin = table.Column<string>(type: "text", nullable: false),
                    figi = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    sector = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    in_portfolio = table.Column<bool>(type: "boolean", nullable: false),
                    in_watch_list = table.Column<bool>(type: "boolean", nullable: false),
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
                name: "shares",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "text", nullable: false),
                    isin = table.Column<string>(type: "text", nullable: false),
                    figi = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    sector = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "timeframes",
                schema: "dict",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_timeframes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dividend_info_entities",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    record_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    declared_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dividend = table.Column<double>(type: "double precision", nullable: false),
                    dividend_prc = table.Column<double>(type: "double precision", nullable: false),
                    share_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dividend_info_entities", x => x.id);
                    table.ForeignKey(
                        name: "fk_dividend_info_entities_share_entities_share_id",
                        column: x => x.share_id,
                        principalSchema: "public",
                        principalTable: "shares",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "analyse_results",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "text", nullable: false),
                    analyse_result_type_id = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    timeframe_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_analyse_results", x => x.id);
                    table.ForeignKey(
                        name: "fk_analyse_results_timeframe_entities_timeframe_id",
                        column: x => x.timeframe_id,
                        principalSchema: "dict",
                        principalTable: "timeframes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "candles",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "text", nullable: false),
                    open = table.Column<double>(type: "double precision", nullable: false),
                    close = table.Column<double>(type: "double precision", nullable: false),
                    high = table.Column<double>(type: "double precision", nullable: false),
                    low = table.Column<double>(type: "double precision", nullable: false),
                    volume = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_complete = table.Column<bool>(type: "boolean", nullable: false),
                    timeframe_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_candles", x => x.id);
                    table.ForeignKey(
                        name: "fk_candles_timeframe_entities_timeframe_id",
                        column: x => x.timeframe_id,
                        principalSchema: "dict",
                        principalTable: "timeframes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "dict",
                table: "timeframes",
                columns: new[] { "id", "created_at", "deleted_at", "description", "is_deleted", "name", "updated_at" },
                values: new object[,]
                {
                    { new Guid("827adf38-2f99-4066-ba5c-33a646d2767b"), new DateTime(2024, 10, 27, 18, 51, 53, 582, DateTimeKind.Utc).AddTicks(9926), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "1 час", false, "H", new DateTime(2024, 10, 27, 18, 51, 53, 582, DateTimeKind.Utc).AddTicks(9927) },
                    { new Guid("eaa80987-548b-474d-8882-9003a10db167"), new DateTime(2024, 10, 27, 18, 51, 53, 582, DateTimeKind.Utc).AddTicks(9858), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "1 день", false, "D", new DateTime(2024, 10, 27, 18, 51, 53, 582, DateTimeKind.Utc).AddTicks(9861) }
                });

            migrationBuilder.CreateIndex(
                name: "ix_analyse_results_ticker",
                schema: "storage",
                table: "analyse_results",
                column: "ticker");

            migrationBuilder.CreateIndex(
                name: "ix_analyse_results_timeframe_id",
                schema: "storage",
                table: "analyse_results",
                column: "timeframe_id");

            migrationBuilder.CreateIndex(
                name: "ix_candles_ticker",
                schema: "storage",
                table: "candles",
                column: "ticker");

            migrationBuilder.CreateIndex(
                name: "ix_candles_timeframe_id",
                schema: "storage",
                table: "candles",
                column: "timeframe_id");

            migrationBuilder.CreateIndex(
                name: "ix_dividend_info_entities_share_id",
                schema: "public",
                table: "dividend_info_entities",
                column: "share_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "analyse_results",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "bonds",
                schema: "public");

            migrationBuilder.DropTable(
                name: "candles",
                schema: "storage");

            migrationBuilder.DropTable(
                name: "dividend_info_entities",
                schema: "public");

            migrationBuilder.DropTable(
                name: "timeframes",
                schema: "dict");

            migrationBuilder.DropTable(
                name: "shares",
                schema: "public");
        }
    }
}
