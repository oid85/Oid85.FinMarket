using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameTableDividendInfos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_dividend_info_entities",
                schema: "public",
                table: "dividend_info_entities");

            migrationBuilder.RenameTable(
                name: "dividend_info_entities",
                schema: "public",
                newName: "dividend_infos",
                newSchema: "public");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                schema: "public",
                table: "dividend_infos",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "pk_dividend_infos",
                schema: "public",
                table: "dividend_infos",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_dividend_infos",
                schema: "public",
                table: "dividend_infos");

            migrationBuilder.RenameTable(
                name: "dividend_infos",
                schema: "public",
                newName: "dividend_info_entities",
                newSchema: "public");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                schema: "public",
                table: "dividend_info_entities",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AddPrimaryKey(
                name: "pk_dividend_info_entities",
                schema: "public",
                table: "dividend_info_entities",
                column: "id");
        }
    }
}
