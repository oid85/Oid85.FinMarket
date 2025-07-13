using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SetMaxLenghtNameShareMultiplicatorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "public",
                table: "share_multiplicators",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "public",
                table: "share_multiplicators",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4923), new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4923) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4930), new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4931) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4915), new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4915) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4933), new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4933) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4920), new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4920) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4917), new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4918) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4928), new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4928) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4836), new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4838) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4912), new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4912) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4925), new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4926) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4904), new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4905) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4909), new DateTime(2025, 7, 13, 9, 7, 20, 667, DateTimeKind.Utc).AddTicks(4909) });
        }
    }
}
