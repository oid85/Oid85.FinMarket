using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditMultiplicatorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "beta",
                schema: "public",
                table: "multiplicators");

            migrationBuilder.DropColumn(
                name: "bv",
                schema: "public",
                table: "multiplicators");

            migrationBuilder.DropColumn(
                name: "ebitda",
                schema: "public",
                table: "multiplicators");

            migrationBuilder.DropColumn(
                name: "eps",
                schema: "public",
                table: "multiplicators");

            migrationBuilder.DropColumn(
                name: "ev_to_ebitda",
                schema: "public",
                table: "multiplicators");

            migrationBuilder.DropColumn(
                name: "free_cash_flow",
                schema: "public",
                table: "multiplicators");

            migrationBuilder.DropColumn(
                name: "market_capitalization",
                schema: "public",
                table: "multiplicators");

            migrationBuilder.DropColumn(
                name: "net_debt",
                schema: "public",
                table: "multiplicators");

            migrationBuilder.DropColumn(
                name: "net_debt_to_ebitda",
                schema: "public",
                table: "multiplicators");

            migrationBuilder.DropColumn(
                name: "net_interest_margin",
                schema: "public",
                table: "multiplicators");

            migrationBuilder.RenameColumn(
                name: "revenue",
                schema: "public",
                table: "multiplicators",
                newName: "Revenue");

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
                name: "total_shares_ap",
                schema: "public",
                table: "multiplicators",
                newName: "Ps");

            migrationBuilder.RenameColumn(
                name: "total_shares_ao",
                schema: "public",
                table: "multiplicators",
                newName: "NetDebtEbitda");

            migrationBuilder.RenameColumn(
                name: "total_debt_to_ebitda",
                schema: "public",
                table: "multiplicators",
                newName: "MarketCap");

            migrationBuilder.RenameColumn(
                name: "total_debt",
                schema: "public",
                table: "multiplicators",
                newName: "EvEbitda");

            migrationBuilder.RenameColumn(
                name: "ticker_ap",
                schema: "public",
                table: "multiplicators",
                newName: "ticker");

            migrationBuilder.RenameColumn(
                name: "ticker_ao",
                schema: "public",
                table: "multiplicators",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "roe",
                schema: "public",
                table: "multiplicators",
                newName: "EbitdaMargin");

            migrationBuilder.RenameColumn(
                name: "roa",
                schema: "public",
                table: "multiplicators",
                newName: "DdNetIncome");

            migrationBuilder.RenameColumn(
                name: "pbv",
                schema: "public",
                table: "multiplicators",
                newName: "DdAp");

            migrationBuilder.RenameColumn(
                name: "operating_income",
                schema: "public",
                table: "multiplicators",
                newName: "DdAo");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Revenue",
                schema: "public",
                table: "multiplicators",
                newName: "revenue");

            migrationBuilder.RenameColumn(
                name: "Pe",
                schema: "public",
                table: "multiplicators",
                newName: "pe");

            migrationBuilder.RenameColumn(
                name: "Pb",
                schema: "public",
                table: "multiplicators",
                newName: "pb");

            migrationBuilder.RenameColumn(
                name: "Ev",
                schema: "public",
                table: "multiplicators",
                newName: "ev");

            migrationBuilder.RenameColumn(
                name: "NetIncome",
                schema: "public",
                table: "multiplicators",
                newName: "net_income");

            migrationBuilder.RenameColumn(
                name: "ticker",
                schema: "public",
                table: "multiplicators",
                newName: "ticker_ap");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "public",
                table: "multiplicators",
                newName: "ticker_ao");

            migrationBuilder.RenameColumn(
                name: "Ps",
                schema: "public",
                table: "multiplicators",
                newName: "total_shares_ap");

            migrationBuilder.RenameColumn(
                name: "NetDebtEbitda",
                schema: "public",
                table: "multiplicators",
                newName: "total_shares_ao");

            migrationBuilder.RenameColumn(
                name: "MarketCap",
                schema: "public",
                table: "multiplicators",
                newName: "total_debt_to_ebitda");

            migrationBuilder.RenameColumn(
                name: "EvEbitda",
                schema: "public",
                table: "multiplicators",
                newName: "total_debt");

            migrationBuilder.RenameColumn(
                name: "EbitdaMargin",
                schema: "public",
                table: "multiplicators",
                newName: "roe");

            migrationBuilder.RenameColumn(
                name: "DdNetIncome",
                schema: "public",
                table: "multiplicators",
                newName: "roa");

            migrationBuilder.RenameColumn(
                name: "DdAp",
                schema: "public",
                table: "multiplicators",
                newName: "pbv");

            migrationBuilder.RenameColumn(
                name: "DdAo",
                schema: "public",
                table: "multiplicators",
                newName: "operating_income");

            migrationBuilder.AddColumn<double>(
                name: "beta",
                schema: "public",
                table: "multiplicators",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "bv",
                schema: "public",
                table: "multiplicators",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ebitda",
                schema: "public",
                table: "multiplicators",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "eps",
                schema: "public",
                table: "multiplicators",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ev_to_ebitda",
                schema: "public",
                table: "multiplicators",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "free_cash_flow",
                schema: "public",
                table: "multiplicators",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "market_capitalization",
                schema: "public",
                table: "multiplicators",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "net_debt",
                schema: "public",
                table: "multiplicators",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "net_debt_to_ebitda",
                schema: "public",
                table: "multiplicators",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "net_interest_margin",
                schema: "public",
                table: "multiplicators",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8829), new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8829) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8837), new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8837) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8820), new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8820) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8839), new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8839) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8825), new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8826) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8823), new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8823) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8834), new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8834) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8746), new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8748) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8817), new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8817) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8832), new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8832) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8809), new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8809) });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8814), new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8814) });
        }
    }
}
