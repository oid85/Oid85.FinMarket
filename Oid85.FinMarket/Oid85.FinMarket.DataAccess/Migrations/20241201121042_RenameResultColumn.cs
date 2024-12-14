using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameResultColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "analyse_result_type_id",
                schema: "storage",
                table: "analyse_results",
                newName: "result");

            migrationBuilder.AddColumn<bool>(
                name: "floating_coupon_flag",
                schema: "public",
                table: "bonds",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "maturity_date",
                schema: "public",
                table: "bonds",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "nkd",
                schema: "public",
                table: "bonds",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "bond_coupons",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "text", nullable: false),
                    coupon_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    coupon_number = table.Column<long>(type: "bigint", nullable: false),
                    coupon_period = table.Column<int>(type: "integer", nullable: false),
                    coupon_start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    coupon_end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    pay_one_bond = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bond_coupons", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bond_coupons",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "floating_coupon_flag",
                schema: "public",
                table: "bonds");

            migrationBuilder.DropColumn(
                name: "maturity_date",
                schema: "public",
                table: "bonds");

            migrationBuilder.DropColumn(
                name: "nkd",
                schema: "public",
                table: "bonds");

            migrationBuilder.RenameColumn(
                name: "result",
                schema: "storage",
                table: "analyse_results",
                newName: "analyse_result_type_id");
        }
    }
}
