using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MoveToDefaultSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "strategy_signals",
                schema: "storage",
                newName: "strategy_signals",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "spreads",
                schema: "storage",
                newName: "spreads",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "optimization_results",
                schema: "storage",
                newName: "optimization_results",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "market_events",
                schema: "storage",
                newName: "market_events",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "hourly_candles",
                schema: "storage",
                newName: "hourly_candles",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "fear_greed_index",
                schema: "storage",
                newName: "fear_greed_index",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "daily_candles",
                schema: "storage",
                newName: "daily_candles",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "backtest_results",
                schema: "storage",
                newName: "backtest_results",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "analyse_results",
                schema: "storage",
                newName: "analyse_results",
                newSchema: "public");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1896), new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1897) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1905), new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1905) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1888), new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1888) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1907), new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1907) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1894), new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1894) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1890), new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1891) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1902), new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1902) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1812), new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1813) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1885), new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1885) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1899), new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1899) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1878), new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1879) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1882), new DateTime(2025, 7, 15, 5, 21, 51, 649, DateTimeKind.Utc).AddTicks(1882) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "storage");

            migrationBuilder.RenameTable(
                name: "strategy_signals",
                schema: "public",
                newName: "strategy_signals",
                newSchema: "storage");

            migrationBuilder.RenameTable(
                name: "spreads",
                schema: "public",
                newName: "spreads",
                newSchema: "storage");

            migrationBuilder.RenameTable(
                name: "optimization_results",
                schema: "public",
                newName: "optimization_results",
                newSchema: "storage");

            migrationBuilder.RenameTable(
                name: "market_events",
                schema: "public",
                newName: "market_events",
                newSchema: "storage");

            migrationBuilder.RenameTable(
                name: "hourly_candles",
                schema: "public",
                newName: "hourly_candles",
                newSchema: "storage");

            migrationBuilder.RenameTable(
                name: "fear_greed_index",
                schema: "public",
                newName: "fear_greed_index",
                newSchema: "storage");

            migrationBuilder.RenameTable(
                name: "daily_candles",
                schema: "public",
                newName: "daily_candles",
                newSchema: "storage");

            migrationBuilder.RenameTable(
                name: "backtest_results",
                schema: "public",
                newName: "backtest_results",
                newSchema: "storage");

            migrationBuilder.RenameTable(
                name: "analyse_results",
                schema: "public",
                newName: "analyse_results",
                newSchema: "storage");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(8033), new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(8033) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(8041), new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(8042) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(8023), new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(8023) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(8044), new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(8044) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(8030), new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(8031) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(8028), new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(8028) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(8039), new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(8039) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(7895), new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(7897) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(7970), new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(7970) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(8036), new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(8036) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(7963), new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(7963) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(7967), new DateTime(2025, 7, 13, 10, 17, 9, 214, DateTimeKind.Utc).AddTicks(7967) });
        }
    }
}
