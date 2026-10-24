using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddProcessedPlainTextToExpressionSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "processed_plain_text",
                table: "expression_sections",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "checkin_stages",
                columns: new[] { "id", "description", "name" },
                values: new object[] { 16, "The player has not checked in yet for this event", "Awaitin Checkin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "checkin_stages",
                keyColumn: "id",
                keyValue: 16);

            migrationBuilder.DropColumn(
                name: "processed_plain_text",
                table: "expression_sections");
        }
    }
}
