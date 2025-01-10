using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsToSpreads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "price_difference_average",
                schema: "storage",
                table: "spreads",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "price_difference_average_prc",
                schema: "storage",
                table: "spreads",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price_difference_average",
                schema: "storage",
                table: "spreads");

            migrationBuilder.DropColumn(
                name: "price_difference_average_prc",
                schema: "storage",
                table: "spreads");
        }
    }
}
