using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class FixBlessingLevelRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_blessing_level_blessing_blessing_id",
                table: "blessing_level");

            migrationBuilder.AddForeignKey(
                name: "FK_blessing_level_blessing_blessing_id",
                table: "blessing_level",
                column: "blessing_id",
                principalTable: "blessing",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_blessing_level_blessing_blessing_id",
                table: "blessing_level");

            migrationBuilder.AddForeignKey(
                name: "FK_blessing_level_blessing_blessing_id",
                table: "blessing_level",
                column: "blessing_id",
                principalTable: "blessing",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
