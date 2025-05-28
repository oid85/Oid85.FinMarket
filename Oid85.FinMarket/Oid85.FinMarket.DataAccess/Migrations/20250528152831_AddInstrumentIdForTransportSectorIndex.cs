using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddInstrumentIdForTransportSectorIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "public",
                table: "instruments",
                columns: new[] { "id", "instrument_id", "name", "sector", "ticker", "type" },
                values: new object[] { new Guid("e5d093cd-e00d-4603-9c81-c33aa0a1bbc6"), new Guid("b0d7ac17-6042-48ab-ad79-076bf950e451"), "Transport Sector Index", "", "TRSI", "Index" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "instruments",
                keyColumn: "id",
                keyValue: new Guid("e5d093cd-e00d-4603-9c81-c33aa0a1bbc6"));
        }
    }
}
