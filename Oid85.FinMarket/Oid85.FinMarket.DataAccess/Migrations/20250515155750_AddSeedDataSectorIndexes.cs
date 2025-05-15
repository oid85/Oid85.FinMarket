using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedDataSectorIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "public",
                table: "instruments",
                columns: new[] { "id", "instrument_id", "name", "sector", "ticker", "type" },
                values: new object[,]
                {
                    { new Guid("271bb1d1-4476-4638-bae8-0cac6f7179ad"), new Guid("8a9ca1af-e4fb-4138-9695-0fccec865480"), "Finance Sector Index", "", "FSI", "Index" },
                    { new Guid("3a20bcb8-6be5-4510-b91a-f3096918686c"), new Guid("8a923a18-1dda-46a8-b163-15491d62314a"), "Banks Sector Index", "", "BSI", "Index" },
                    { new Guid("712e7169-5953-46a0-829e-400b9015a56d"), new Guid("42ef3680-1ea0-42e3-b587-a0abe1a23dc7"), "IT Sector Index", "", "ITSI", "Index" },
                    { new Guid("97094d1f-8426-44fc-a25d-a556ec9c3a97"), new Guid("da5ae557-fb6c-4cc2-886d-4bc4aedce12a"), "Non Ferrous Metallurgy Sector Index", "", "NFMSI", "Index" },
                    { new Guid("abe702e9-0271-4622-9f52-4de2da88ebfc"), new Guid("299bc8a4-db16-4fef-8adb-f9cfe138f0eb"), "Energ Sector Index", "", "ESI", "Index" },
                    { new Guid("c3db74d9-048b-4fe7-9abe-3d67a1b4010f"), new Guid("2faf88b4-037f-40fc-ade1-5b72452d9c15"), "Mining Sector Index", "", "MSI", "Index" },
                    { new Guid("d05f0a65-9d0c-42dc-8dd1-130ba5bfeb3e"), new Guid("b916084e-dcee-440f-ac44-6240913753f6"), "Housing And Utilities Sector Index", "", "HUSI", "Index" },
                    { new Guid("d2615eb5-c224-4e7d-9bb7-8bfa4e1351ea"), new Guid("8b18fbbf-5d4e-4578-8586-1a104ad1dcb1"), "IronAndSteelIndustry Sector Index", "", "ISISI", "Index" },
                    { new Guid("d9edc8e2-df33-484d-b509-74b55c44396d"), new Guid("f039936b-5183-46d1-a262-feff6b83a377"), "Telecom Sector Index", "", "TSI", "Index" },
                    { new Guid("e0683c7c-68b0-4d9a-a3b4-8f94086df49f"), new Guid("6d35ac04-9c84-4c4d-b2fa-7aae12b2fcdd"), "Retail Sector Index", "", "RSI", "Index" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "instruments",
                keyColumn: "id",
                keyValue: new Guid("271bb1d1-4476-4638-bae8-0cac6f7179ad"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "instruments",
                keyColumn: "id",
                keyValue: new Guid("3a20bcb8-6be5-4510-b91a-f3096918686c"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "instruments",
                keyColumn: "id",
                keyValue: new Guid("712e7169-5953-46a0-829e-400b9015a56d"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "instruments",
                keyColumn: "id",
                keyValue: new Guid("97094d1f-8426-44fc-a25d-a556ec9c3a97"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "instruments",
                keyColumn: "id",
                keyValue: new Guid("abe702e9-0271-4622-9f52-4de2da88ebfc"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "instruments",
                keyColumn: "id",
                keyValue: new Guid("c3db74d9-048b-4fe7-9abe-3d67a1b4010f"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "instruments",
                keyColumn: "id",
                keyValue: new Guid("d05f0a65-9d0c-42dc-8dd1-130ba5bfeb3e"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "instruments",
                keyColumn: "id",
                keyValue: new Guid("d2615eb5-c224-4e7d-9bb7-8bfa4e1351ea"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "instruments",
                keyColumn: "id",
                keyValue: new Guid("d9edc8e2-df33-484d-b509-74b55c44396d"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "instruments",
                keyColumn: "id",
                keyValue: new Guid("e0683c7c-68b0-4d9a-a3b4-8f94086df49f"));
        }
    }
}
