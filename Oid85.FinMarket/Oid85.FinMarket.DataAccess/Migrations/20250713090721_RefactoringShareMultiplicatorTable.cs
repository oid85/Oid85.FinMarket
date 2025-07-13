using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringShareMultiplicatorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_multiplicators",
                schema: "public",
                table: "multiplicators");

            migrationBuilder.RenameTable(
                name: "multiplicators",
                schema: "public",
                newName: "share_multiplicators",
                newSchema: "public");

            migrationBuilder.RenameColumn(
                name: "Revenue",
                schema: "public",
                table: "share_multiplicators",
                newName: "revenue");

            migrationBuilder.RenameColumn(
                name: "Ps",
                schema: "public",
                table: "share_multiplicators",
                newName: "ps");

            migrationBuilder.RenameColumn(
                name: "Pe",
                schema: "public",
                table: "share_multiplicators",
                newName: "pe");

            migrationBuilder.RenameColumn(
                name: "Pb",
                schema: "public",
                table: "share_multiplicators",
                newName: "pb");

            migrationBuilder.RenameColumn(
                name: "Ev",
                schema: "public",
                table: "share_multiplicators",
                newName: "ev");

            migrationBuilder.RenameColumn(
                name: "NetIncome",
                schema: "public",
                table: "share_multiplicators",
                newName: "net_income");

            migrationBuilder.RenameColumn(
                name: "NetDebtEbitda",
                schema: "public",
                table: "share_multiplicators",
                newName: "net_debt_ebitda");

            migrationBuilder.RenameColumn(
                name: "MarketCap",
                schema: "public",
                table: "share_multiplicators",
                newName: "market_cap");

            migrationBuilder.RenameColumn(
                name: "EvEbitda",
                schema: "public",
                table: "share_multiplicators",
                newName: "ev_ebitda");

            migrationBuilder.RenameColumn(
                name: "EbitdaMargin",
                schema: "public",
                table: "share_multiplicators",
                newName: "ebitda_margin");

            migrationBuilder.RenameColumn(
                name: "DdNetIncome",
                schema: "public",
                table: "share_multiplicators",
                newName: "dd_net_income");

            migrationBuilder.RenameColumn(
                name: "DdAp",
                schema: "public",
                table: "share_multiplicators",
                newName: "dd_ap");

            migrationBuilder.RenameColumn(
                name: "DdAo",
                schema: "public",
                table: "share_multiplicators",
                newName: "dd_ao");

            migrationBuilder.AddPrimaryKey(
                name: "PK_share_multiplicators",
                schema: "public",
                table: "share_multiplicators",
                column: "id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_share_multiplicators",
                schema: "public",
                table: "share_multiplicators");

            migrationBuilder.RenameTable(
                name: "share_multiplicators",
                schema: "public",
                newName: "multiplicators",
                newSchema: "public");

            migrationBuilder.RenameColumn(
                name: "revenue",
                schema: "public",
                table: "multiplicators",
                newName: "Revenue");

            migrationBuilder.RenameColumn(
                name: "ps",
                schema: "public",
                table: "multiplicators",
                newName: "Ps");

            migrationBuilder.RenameColumn(
                name: "pe",
                schema: "public",
                table: "multiplicators",
                newName: "Pe");

            migrationBuilder.RenameColumn(
                name: "pb",
                schema: "public",
                table: "multiplicators",
                newName: "Pb");

            migrationBuilder.RenameColumn(
                name: "ev",
                schema: "public",
                table: "multiplicators",
                newName: "Ev");

            migrationBuilder.RenameColumn(
                name: "net_income",
                schema: "public",
                table: "multiplicators",
                newName: "NetIncome");

            migrationBuilder.RenameColumn(
                name: "net_debt_ebitda",
                schema: "public",
                table: "multiplicators",
                newName: "NetDebtEbitda");

            migrationBuilder.RenameColumn(
                name: "market_cap",
                schema: "public",
                table: "multiplicators",
                newName: "MarketCap");

            migrationBuilder.RenameColumn(
                name: "ev_ebitda",
                schema: "public",
                table: "multiplicators",
                newName: "EvEbitda");

            migrationBuilder.RenameColumn(
                name: "ebitda_margin",
                schema: "public",
                table: "multiplicators",
                newName: "EbitdaMargin");

            migrationBuilder.RenameColumn(
                name: "dd_net_income",
                schema: "public",
                table: "multiplicators",
                newName: "DdNetIncome");

            migrationBuilder.RenameColumn(
                name: "dd_ap",
                schema: "public",
                table: "multiplicators",
                newName: "DdAp");

            migrationBuilder.RenameColumn(
                name: "dd_ao",
                schema: "public",
                table: "multiplicators",
                newName: "DdAo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_multiplicators",
                schema: "public",
                table: "multiplicators",
                column: "id");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3438), new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3438) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3446), new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3447) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3397), new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3398) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3449), new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3449) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3434), new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3434) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3400), new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3401) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3444), new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3444) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3321), new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3323) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3394), new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3395) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3441), new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3441) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3386), new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3387) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3391), new DateTime(2025, 7, 13, 6, 42, 38, 378, DateTimeKind.Utc).AddTicks(3392) });
        }
    }
}
