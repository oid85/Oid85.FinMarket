using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "public",
                table: "fin_indexes",
                columns: new[] { "id", "class_code", "created_at", "currency", "deleted_at", "exchange", "figi", "instrument_id", "instrument_kind", "is_deleted", "last_price", "name", "ticker", "updated_at" },
                values: new object[,]
                {
                    { new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"), "", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8829), "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "", new Guid("2faf88b4-037f-40fc-ade1-5b72452d9c15"), "", false, 0.0, "Mining Sector Index", "MSI", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8829) },
                    { new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"), "", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8837), "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "", new Guid("f039936b-5183-46d1-a262-feff6b83a377"), "", false, 0.0, "Telecom Sector Index", "TSI", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8837) },
                    { new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"), "", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8820), "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "", new Guid("b916084e-dcee-440f-ac44-6240913753f6"), "", false, 0.0, "Housing And Utilities Sector Index", "HUSI", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8820) },
                    { new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"), "", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8839), "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "", new Guid("b0d7ac17-6042-48ab-ad79-076bf950e451"), "", false, 0.0, "Transport Sector Index", "TRSI", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8839) },
                    { new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"), "", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8825), "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "", new Guid("42ef3680-1ea0-42e3-b587-a0abe1a23dc7"), "", false, 0.0, "IT Sector Index", "ITSI", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8826) },
                    { new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"), "", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8823), "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "", new Guid("8b18fbbf-5d4e-4578-8586-1a104ad1dcb1"), "", false, 0.0, "IronAndSteelIndustry Sector Index", "ISISI", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8823) },
                    { new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"), "", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8834), "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "", new Guid("6d35ac04-9c84-4c4d-b2fa-7aae12b2fcdd"), "", false, 0.0, "Retail Sector Index", "RSI", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8834) },
                    { new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"), "", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8746), "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "", new Guid("6503b433-233c-4d99-bea6-017e7dcad033"), "", false, 0.0, "Oil and Gas Sector Index", "OGSI", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8748) },
                    { new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"), "", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8817), "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "", new Guid("8a9ca1af-e4fb-4138-9695-0fccec865480"), "", false, 0.0, "Finance Sector Index", "FSI", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8817) },
                    { new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"), "", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8832), "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "", new Guid("da5ae557-fb6c-4cc2-886d-4bc4aedce12a"), "", false, 0.0, "Non Ferrous Metallurgy Sector Index", "NFMSI", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8832) },
                    { new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"), "", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8809), "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "", new Guid("8a923a18-1dda-46a8-b163-15491d62314a"), "", false, 0.0, "Banks Sector Index", "BSI", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8809) },
                    { new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"), "", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8814), "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "", new Guid("299bc8a4-db16-4fef-8adb-f9cfe138f0eb"), "", false, 0.0, "Energ Sector Index", "ESI", new DateTime(2025, 7, 12, 15, 30, 14, 34, DateTimeKind.Utc).AddTicks(8814) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2b063b20-da30-407b-8a5a-1de917d6e889"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("4c968e87-2c9e-43b2-a896-09798c74b082"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("7002e1b6-3709-4b79-9598-0b87d50bdd52"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("8e237dbb-d8df-4599-ad6e-4635cb891ef5"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("923ada63-22f4-429f-b50f-3ac61b2cb457"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("a70dd4da-cb55-47f2-ac00-fefd08bc30db"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "fin_indexes",
                keyColumn: "id",
                keyValue: new Guid("ed81ffb7-166c-4109-aa1f-11ff2021d214"));
        }
    }
}
