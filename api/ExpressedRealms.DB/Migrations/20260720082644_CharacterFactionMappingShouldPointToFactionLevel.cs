using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class CharacterFactionMappingShouldPointToFactionLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_character_faction_mappings_faction_ranks_faction_rank_id",
                table: "character_faction_mappings");

            migrationBuilder.AlterColumn<int>(
                name: "faction_rank_id",
                table: "character_faction_mappings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "faction_level_id",
                table: "character_faction_mappings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_character_faction_mappings_faction_level_id",
                table: "character_faction_mappings",
                column: "faction_level_id");

            migrationBuilder.AddForeignKey(
                name: "fk_character_faction_mappings_faction_levels_faction_level_id",
                table: "character_faction_mappings",
                column: "faction_level_id",
                principalTable: "faction_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_faction_mappings_faction_ranks_faction_rank_id",
                table: "character_faction_mappings",
                column: "faction_rank_id",
                principalTable: "faction_ranks",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_character_faction_mappings_faction_levels_faction_level_id",
                table: "character_faction_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_character_faction_mappings_faction_ranks_faction_rank_id",
                table: "character_faction_mappings");

            migrationBuilder.DropIndex(
                name: "ix_character_faction_mappings_faction_level_id",
                table: "character_faction_mappings");

            migrationBuilder.DropColumn(
                name: "faction_level_id",
                table: "character_faction_mappings");

            migrationBuilder.AlterColumn<int>(
                name: "faction_rank_id",
                table: "character_faction_mappings",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_character_faction_mappings_faction_ranks_faction_rank_id",
                table: "character_faction_mappings",
                column: "faction_rank_id",
                principalTable: "faction_ranks",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
