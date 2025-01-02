using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemaneColumnLastPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "price",
                schema: "public",
                table: "shares",
                newName: "last_price");

            migrationBuilder.RenameColumn(
                name: "price",
                schema: "public",
                table: "futures",
                newName: "last_price");

            migrationBuilder.RenameColumn(
                name: "price",
                schema: "public",
                table: "fin_indexes",
                newName: "last_price");

            migrationBuilder.RenameColumn(
                name: "price",
                schema: "public",
                table: "currencies",
                newName: "last_price");

            migrationBuilder.RenameColumn(
                name: "price",
                schema: "public",
                table: "bonds",
                newName: "last_price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "last_price",
                schema: "public",
                table: "shares",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "last_price",
                schema: "public",
                table: "futures",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "last_price",
                schema: "public",
                table: "fin_indexes",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "last_price",
                schema: "public",
                table: "currencies",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "last_price",
                schema: "public",
                table: "bonds",
                newName: "price");
        }
    }
}
