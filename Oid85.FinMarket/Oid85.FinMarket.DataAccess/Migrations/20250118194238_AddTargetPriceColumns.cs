using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTargetPriceColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "high_target_price",
                schema: "public",
                table: "shares",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "low_target_price",
                schema: "public",
                table: "shares",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "high_target_price",
                schema: "public",
                table: "futures",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "low_target_price",
                schema: "public",
                table: "futures",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "high_target_price",
                schema: "public",
                table: "fin_indexes",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "low_target_price",
                schema: "public",
                table: "fin_indexes",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "high_target_price",
                schema: "public",
                table: "currencies",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "low_target_price",
                schema: "public",
                table: "currencies",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "high_target_price",
                schema: "public",
                table: "bonds",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "low_target_price",
                schema: "public",
                table: "bonds",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "high_target_price",
                schema: "public",
                table: "shares");

            migrationBuilder.DropColumn(
                name: "low_target_price",
                schema: "public",
                table: "shares");

            migrationBuilder.DropColumn(
                name: "high_target_price",
                schema: "public",
                table: "futures");

            migrationBuilder.DropColumn(
                name: "low_target_price",
                schema: "public",
                table: "futures");

            migrationBuilder.DropColumn(
                name: "high_target_price",
                schema: "public",
                table: "fin_indexes");

            migrationBuilder.DropColumn(
                name: "low_target_price",
                schema: "public",
                table: "fin_indexes");

            migrationBuilder.DropColumn(
                name: "high_target_price",
                schema: "public",
                table: "currencies");

            migrationBuilder.DropColumn(
                name: "low_target_price",
                schema: "public",
                table: "currencies");

            migrationBuilder.DropColumn(
                name: "high_target_price",
                schema: "public",
                table: "bonds");

            migrationBuilder.DropColumn(
                name: "low_target_price",
                schema: "public",
                table: "bonds");
        }
    }
}
