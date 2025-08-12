using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class _12082025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "percent_signals",
                schema: "public",
                table: "strategy_signals",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "position_percent_portfolio",
                schema: "public",
                table: "strategy_signals",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9743), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9743) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9751), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9751) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9735), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9735) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9753), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9753) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9741), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9741) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9738), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9738) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9748), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9748) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9616), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9621) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9700), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9701) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9746), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9746) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9694), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9694) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9698), new DateTime(2025, 8, 12, 8, 15, 17, 920, DateTimeKind.Utc).AddTicks(9698) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "percent_signals",
                schema: "public",
                table: "strategy_signals");

            migrationBuilder.DropColumn(
                name: "position_percent_portfolio",
                schema: "public",
                table: "strategy_signals");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1238), new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1238) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1246), new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1246) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1230), new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1230) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1248), new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1249) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1235), new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1235) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1232), new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1232) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1243), new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1244) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1107), new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1110) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1227), new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1227) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1240), new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1241) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1220), new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1220) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1224), new DateTime(2025, 7, 27, 11, 49, 32, 998, DateTimeKind.Utc).AddTicks(1225) });
        }
    }
}
