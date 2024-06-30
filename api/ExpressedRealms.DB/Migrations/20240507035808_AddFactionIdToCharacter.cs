using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddFactionIdToCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FactionId",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_FactionId",
                table: "Characters",
                column: "FactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_ExpressionSections_FactionId",
                table: "Characters",
                column: "FactionId",
                principalTable: "ExpressionSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_ExpressionSections_FactionId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_FactionId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "FactionId",
                table: "Characters");
        }
    }
}
