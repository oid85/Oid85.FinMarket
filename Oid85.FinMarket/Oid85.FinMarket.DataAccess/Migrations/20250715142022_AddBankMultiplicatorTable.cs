using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddBankMultiplicatorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bank_multiplicators",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ticker = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    market_cap = table.Column<double>(type: "double precision", nullable: false),
                    net_income = table.Column<double>(type: "double precision", nullable: false),
                    dd_ao = table.Column<double>(type: "double precision", nullable: false),
                    dd_ap = table.Column<double>(type: "double precision", nullable: false),
                    dd_net_income = table.Column<double>(type: "double precision", nullable: false),
                    pe = table.Column<double>(type: "double precision", nullable: false),
                    pb = table.Column<double>(type: "double precision", nullable: false),
                    net_operating_income = table.Column<double>(type: "double precision", nullable: false),
                    net_interest_margin = table.Column<double>(type: "double precision", nullable: false),
                    roe = table.Column<double>(type: "double precision", nullable: false),
                    roa = table.Column<double>(type: "double precision", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank_multiplicators", x => x.id);
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bank_multiplicators",
                schema: "public");

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
    }
}
