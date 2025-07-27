using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnSlopeAndIntercept : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "dates",
                schema: "public",
                table: "regression_tails",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500000)",
                oldMaxLength: 500000);

            migrationBuilder.AddColumn<double>(
                name: "intercept",
                schema: "public",
                table: "regression_tails",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "slope",
                schema: "public",
                table: "regression_tails",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1015), new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1015) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1022), new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1023) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1007), new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1008) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1025), new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1025) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1012), new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1013) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1010), new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1010) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1020), new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1020) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(931), new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(933) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1005), new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1005) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1017), new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1018) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(997), new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(997) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1001), new DateTime(2025, 7, 27, 5, 36, 45, 250, DateTimeKind.Utc).AddTicks(1001) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "intercept",
                schema: "public",
                table: "regression_tails");

            migrationBuilder.DropColumn(
                name: "slope",
                schema: "public",
                table: "regression_tails");

            migrationBuilder.AlterColumn<string>(
                name: "dates",
                schema: "public",
                table: "regression_tails",
                type: "character varying(500000)",
                maxLength: 500000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7955), new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7955) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7986), new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7987) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7938), new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7938) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7992), new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7992) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7950), new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7950) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7944), new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7944) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7980), new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7981) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7767), new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7770) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7932), new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7932) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7974), new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7975) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7862), new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7862) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7922), new DateTime(2025, 7, 25, 5, 40, 40, 454, DateTimeKind.Utc).AddTicks(7923) });
        }
    }
}
