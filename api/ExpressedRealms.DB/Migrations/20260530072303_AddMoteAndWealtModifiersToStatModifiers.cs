using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddMoteAndWealtModifiersToStatModifiers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "stat_modifiers",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 24, "Prima / Void" },
                    { 25, "Wealth Level" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "stat_modifiers",
                keyColumn: "id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "stat_modifiers",
                keyColumn: "id",
                keyValue: 25);
        }
    }
}
