using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Oid85.FinMarket.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "_1D",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    datetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    open = table.Column<double>(type: "double precision", nullable: false),
                    close = table.Column<double>(type: "double precision", nullable: false),
                    high = table.Column<double>(type: "double precision", nullable: false),
                    low = table.Column<double>(type: "double precision", nullable: false),
                    volume = table.Column<long>(type: "bigint", nullable: false),
                    ticker = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__1D", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "_1H",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    datetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    open = table.Column<double>(type: "double precision", nullable: false),
                    close = table.Column<double>(type: "double precision", nullable: false),
                    high = table.Column<double>(type: "double precision", nullable: false),
                    low = table.Column<double>(type: "double precision", nullable: false),
                    volume = table.Column<long>(type: "bigint", nullable: false),
                    ticker = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__1H", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "_1M",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    datetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    open = table.Column<double>(type: "double precision", nullable: false),
                    close = table.Column<double>(type: "double precision", nullable: false),
                    high = table.Column<double>(type: "double precision", nullable: false),
                    low = table.Column<double>(type: "double precision", nullable: false),
                    volume = table.Column<long>(type: "bigint", nullable: false),
                    ticker = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__1M", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "assets",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticker = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    figi = table.Column<string>(type: "text", nullable: false),
                    sector = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assets", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "assets",
                columns: new[] { "id", "figi", "name", "sector", "ticker" },
                values: new object[,]
                {
                    { new Guid("007dad9d-9dd6-4f5c-8cba-ee634db20be4"), "BBG004S688H3", "Акрон, акция обыкновенная", "Химия, удобрения", "AKRN" },
                    { new Guid("84072df0-85d8-4135-be2d-f508cd5c01eb"), "BBG004S683X6", "Аэрофлот, акция обыкновенная", "Транспорт", "AFLT" },
                    { new Guid("a4df3b20-a222-43cf-851e-bd3b705c0bd4"), "BBG004S68B40", "Алроса, акция обыкновенная", "Горнодобывающие", "ALRS" },
                    { new Guid("bf5d3d04-bfaa-4623-9a65-7431d00b2a86"), "BBG004730N97", "Сбербанк, акция обыкновенная", "Банки", "SBER" },
                    { new Guid("da48943a-eed9-42ce-b0d6-c7a6ccdf3fc2"), "BBG002W2FT78", "Абрау-Дюрсо, акция обыкновенная", "Агропром и Пищепром", "ABRD" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_1D",
                schema: "public");

            migrationBuilder.DropTable(
                name: "_1H",
                schema: "public");

            migrationBuilder.DropTable(
                name: "_1M",
                schema: "public");

            migrationBuilder.DropTable(
                name: "assets",
                schema: "public");
        }
    }
}
