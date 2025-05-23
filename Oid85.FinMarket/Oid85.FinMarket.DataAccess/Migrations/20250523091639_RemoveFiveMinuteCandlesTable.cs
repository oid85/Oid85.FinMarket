using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFiveMinuteCandlesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "five_minute_candles",
                schema: "storage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "five_minute_candles",
                schema: "storage",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    close = table.Column<double>(type: "double precision", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    datetime = table.Column<long>(type: "bigint", nullable: false),
                    high = table.Column<double>(type: "double precision", nullable: false),
                    instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_complete = table.Column<bool>(type: "boolean", nullable: false),
                    low = table.Column<double>(type: "double precision", nullable: false),
                    open = table.Column<double>(type: "double precision", nullable: false),
                    time = table.Column<TimeOnly>(type: "time", nullable: false),
                    volume = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_five_minute_candles", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_five_minute_candles_instrument_id",
                schema: "storage",
                table: "five_minute_candles",
                column: "instrument_id");
        }
    }
}
