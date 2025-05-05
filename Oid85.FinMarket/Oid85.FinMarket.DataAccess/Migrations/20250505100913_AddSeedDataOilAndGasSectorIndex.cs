using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedDataOilAndGasSectorIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "public",
                table: "instruments",
                columns: new[] { "id", "instrument_id", "name", "sector", "ticker", "type" },
                values: new object[] { new Guid("05804fbf-35a1-481c-bbf2-4acfc3996da3"), new Guid("6503b433-233c-4d99-bea6-017e7dcad033"), "Oil and Gas Sector Index", "", "OGSI", "Index" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "instruments",
                keyColumn: "id",
                keyValue: new Guid("05804fbf-35a1-481c-bbf2-4acfc3996da3"));
        }
    }
}
