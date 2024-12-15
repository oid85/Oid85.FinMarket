using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DeleteIsActiveColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                schema: "public",
                table: "shares");

            migrationBuilder.DropColumn(
                name: "is_active",
                schema: "public",
                table: "bonds");

            migrationBuilder.AddColumn<double>(
                name: "price",
                schema: "public",
                table: "shares",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "price",
                schema: "public",
                table: "bonds",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "price",
                schema: "public",
                table: "bond_coupons",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "futures",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    figi = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    in_portfolio = table.Column<bool>(type: "boolean", nullable: false),
                    in_watch_list = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_futures", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "futures",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "price",
                schema: "public",
                table: "shares");

            migrationBuilder.DropColumn(
                name: "price",
                schema: "public",
                table: "bonds");

            migrationBuilder.DropColumn(
                name: "price",
                schema: "public",
                table: "bond_coupons");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                schema: "public",
                table: "shares",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                schema: "public",
                table: "bonds",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
