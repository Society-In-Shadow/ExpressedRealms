using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddLegacyPublishOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "expression_publish_status",
                columns: new[] { "id", "description", "name" },
                values: new object[] { 5, "Everyone can view and create characters with this expression, but cannot be used as a primary character", "Legacy" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "expression_publish_status",
                keyColumn: "id",
                keyValue: 5);
        }
    }
}
