using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddPrimaryAndSecondaryProgressions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "primary_progression_id",
                table: "Characters",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "secondary_progression_id",
                table: "Characters",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_primary_progression_id",
                table: "Characters",
                column: "primary_progression_id");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_secondary_progression_id",
                table: "Characters",
                column: "secondary_progression_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_progression_path_primary_progression_id",
                table: "Characters",
                column: "primary_progression_id",
                principalTable: "progression_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_progression_path_secondary_progression_id",
                table: "Characters",
                column: "secondary_progression_id",
                principalTable: "progression_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_progression_path_primary_progression_id",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Characters_progression_path_secondary_progression_id",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_primary_progression_id",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_secondary_progression_id",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "primary_progression_id",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "secondary_progression_id",
                table: "Characters");
        }
    }
}
