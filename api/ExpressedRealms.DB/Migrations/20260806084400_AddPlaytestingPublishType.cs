using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddPlaytestingPublishType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "expression_publish_status",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 4, "Everyone can view and create characters with this expression, but cannot be used as a primary character", "Playtesting" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "expression_publish_status",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "expression_publish_status",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "expression_publish_status",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "expression_publish_status",
                keyColumn: "id",
                keyValue: 4);
        }
    }
}
