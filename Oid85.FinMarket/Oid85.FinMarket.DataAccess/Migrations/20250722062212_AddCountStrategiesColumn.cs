using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCountStrategiesColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "count_strategies",
                schema: "public",
                table: "strategy_signals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7817), new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7817) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7825), new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7826) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7809), new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7809) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7828), new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7828) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7814), new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7814) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7812), new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7812) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7823), new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7823) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7730), new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7732) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7806), new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7806) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7820), new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7820) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7800), new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7800) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7803), new DateTime(2025, 7, 22, 6, 22, 11, 734, DateTimeKind.Utc).AddTicks(7804) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "count_strategies",
                schema: "public",
                table: "strategy_signals");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3301), new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3301) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3309), new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3309) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3293), new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3293) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3311), new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3311) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3298), new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3298) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3296), new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3296) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3306), new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3306) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3214), new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3217) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3291), new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3291) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3303), new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3303) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3285), new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3286) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3288), new DateTime(2025, 7, 20, 15, 34, 23, 309, DateTimeKind.Utc).AddTicks(3288) });
        }
    }
}
