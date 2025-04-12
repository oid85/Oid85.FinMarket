using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oid85.FinMarket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddBvColumnToMultiplicatorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "bv",
                schema: "public",
                table: "multiplicators",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bv",
                schema: "public",
                table: "multiplicators");
        }
    }
}
