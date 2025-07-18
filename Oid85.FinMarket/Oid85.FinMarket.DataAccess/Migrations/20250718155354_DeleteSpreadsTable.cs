using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DeleteSpreadsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "spreads",
                schema: "public");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6379), new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6379) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6387), new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6387) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6372), new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6372) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6389), new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6390) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6377), new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6377) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6374), new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6375) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6385), new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6385) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6290), new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6291) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6369), new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6369) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6381), new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6382) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6363), new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6363) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6366), new DateTime(2025, 7, 18, 15, 53, 53, 557, DateTimeKind.Utc).AddTicks(6367) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "spreads",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    datetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    first_instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_instrument_price = table.Column<double>(type: "double precision", nullable: false),
                    first_instrument_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    first_instrument_ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    funding = table.Column<double>(type: "double precision", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    multiplier = table.Column<double>(type: "double precision", nullable: false),
                    price_difference = table.Column<double>(type: "double precision", nullable: false),
                    price_difference_average = table.Column<double>(type: "double precision", nullable: false),
                    price_difference_average_prc = table.Column<double>(type: "double precision", nullable: false),
                    price_difference_prc = table.Column<double>(type: "double precision", nullable: false),
                    second_instrument_id = table.Column<Guid>(type: "uuid", nullable: false),
                    second_instrument_price = table.Column<double>(type: "double precision", nullable: false),
                    second_instrument_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    second_instrument_ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    price_position = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spreads", x => x.id);
                });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6435), new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6436) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6444), new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6444) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6427), new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6427) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6478), new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6479) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6433), new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6433) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6430), new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6430) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6441), new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6441) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6347), new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6352) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6424), new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6425) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6438), new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6438) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6417), new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6417) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6421), new DateTime(2025, 7, 15, 14, 20, 21, 680, DateTimeKind.Utc).AddTicks(6421) });
        }
    }
}
