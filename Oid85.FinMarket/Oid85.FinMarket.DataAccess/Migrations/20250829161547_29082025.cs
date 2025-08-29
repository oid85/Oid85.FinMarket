using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class _29082025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ticker2",
                schema: "public",
                table: "regression_tails",
                newName: "ticker_second");

            migrationBuilder.RenameColumn(
                name: "ticker1",
                schema: "public",
                table: "regression_tails",
                newName: "ticker_first");

            migrationBuilder.RenameColumn(
                name: "ticker2",
                schema: "public",
                table: "correlations",
                newName: "ticker_second");

            migrationBuilder.RenameColumn(
                name: "ticker1",
                schema: "public",
                table: "correlations",
                newName: "ticker_first");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(98), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(98) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(107), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(107) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(87), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(87) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(109), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(109) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(96), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(96) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(90), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(90) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(104), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(104) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 250, DateTimeKind.Utc).AddTicks(9990), new DateTime(2025, 8, 29, 16, 15, 47, 250, DateTimeKind.Utc).AddTicks(9992) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(59), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(59) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(102), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(102) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(53), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(53) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(56), new DateTime(2025, 8, 29, 16, 15, 47, 251, DateTimeKind.Utc).AddTicks(56) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ticker_second",
                schema: "public",
                table: "regression_tails",
                newName: "ticker2");

            migrationBuilder.RenameColumn(
                name: "ticker_first",
                schema: "public",
                table: "regression_tails",
                newName: "ticker1");

            migrationBuilder.RenameColumn(
                name: "ticker_second",
                schema: "public",
                table: "correlations",
                newName: "ticker2");

            migrationBuilder.RenameColumn(
                name: "ticker_first",
                schema: "public",
                table: "correlations",
                newName: "ticker1");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3862), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3863) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3913), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3914) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3855), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3855) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3917), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3917) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3860), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3860) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3857), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3858) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3868), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3868) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3775), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3777) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3852), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3853) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3865), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3865) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3846), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3846) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3850), new DateTime(2025, 8, 25, 8, 42, 4, 832, DateTimeKind.Utc).AddTicks(3850) });
        }
    }
}
