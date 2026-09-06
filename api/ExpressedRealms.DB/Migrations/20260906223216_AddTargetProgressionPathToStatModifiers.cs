using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddTargetProgressionPathToStatModifiers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "target_progression_level_id",
                table: "stat_group_mappings",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_stat_group_mappings_target_progression_level_id",
                table: "stat_group_mappings",
                column: "target_progression_level_id");

            migrationBuilder.AddForeignKey(
                name: "fk_stat_group_mappings_progression_level_target_progression_le",
                table: "stat_group_mappings",
                column: "target_progression_level_id",
                principalTable: "progression_level",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_stat_group_mappings_progression_level_target_progression_le",
                table: "stat_group_mappings");

            migrationBuilder.DropIndex(
                name: "ix_stat_group_mappings_target_progression_level_id",
                table: "stat_group_mappings");

            migrationBuilder.DropColumn(
                name: "target_progression_level_id",
                table: "stat_group_mappings");
        }
    }
}
