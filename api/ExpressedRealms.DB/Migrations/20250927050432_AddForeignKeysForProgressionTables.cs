using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeysForProgressionTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_progression_path_expression_id",
                table: "progression_path",
                column: "expression_id");

            migrationBuilder.CreateIndex(
                name: "IX_progression_level_progression_path_id",
                table: "progression_level",
                column: "progression_path_id");

            migrationBuilder.AddForeignKey(
                name: "FK_progression_level_progression_path_progression_path_id",
                table: "progression_level",
                column: "progression_path_id",
                principalTable: "progression_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_progression_path_expression_expression_id",
                table: "progression_path",
                column: "expression_id",
                principalTable: "expression",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_progression_level_progression_path_progression_path_id",
                table: "progression_level");

            migrationBuilder.DropForeignKey(
                name: "FK_progression_path_expression_expression_id",
                table: "progression_path");

            migrationBuilder.DropIndex(
                name: "IX_progression_path_expression_id",
                table: "progression_path");

            migrationBuilder.DropIndex(
                name: "IX_progression_level_progression_path_id",
                table: "progression_level");
        }
    }
}
