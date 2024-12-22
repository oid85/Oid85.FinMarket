using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Refactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "in_irus_index",
                schema: "public",
                table: "shares");

            migrationBuilder.DropColumn(
                name: "in_portfolio",
                schema: "public",
                table: "shares");

            migrationBuilder.DropColumn(
                name: "in_portfolio",
                schema: "public",
                table: "futures");

            migrationBuilder.DropColumn(
                name: "in_portfolio",
                schema: "public",
                table: "bonds");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "in_irus_index",
                schema: "public",
                table: "shares",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "in_portfolio",
                schema: "public",
                table: "shares",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "in_portfolio",
                schema: "public",
                table: "futures",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "in_portfolio",
                schema: "public",
                table: "bonds",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
