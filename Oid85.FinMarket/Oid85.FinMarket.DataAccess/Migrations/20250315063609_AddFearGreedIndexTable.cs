using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddFearGreedIndexTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fear_greed_index",
                schema: "storage");
        }
    }
}
