using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOldFactionIdInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_characters_expression_sections_faction_id",
                table: "characters");

            migrationBuilder.DropIndex(
                name: "ix_characters_faction_id",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "faction_id",
                table: "characters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "checkin_stages",
                keyColumn: "id",
                keyValue: 16);

            migrationBuilder.AddColumn<int>(
                name: "faction_id",
                table: "characters",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_characters_faction_id",
                table: "characters",
                column: "faction_id");

            migrationBuilder.AddForeignKey(
                name: "fk_characters_expression_sections_faction_id",
                table: "characters",
                column: "faction_id",
                principalTable: "expression_sections",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
