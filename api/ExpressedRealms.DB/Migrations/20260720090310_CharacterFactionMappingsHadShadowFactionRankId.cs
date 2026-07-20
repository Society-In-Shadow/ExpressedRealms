using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class CharacterFactionMappingsHadShadowFactionRankId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_character_faction_mappings_faction_ranks_faction_rank_id",
                table: "character_faction_mappings");

            migrationBuilder.DropIndex(
                name: "ix_character_faction_mappings_faction_rank_id",
                table: "character_faction_mappings");

            migrationBuilder.DropColumn(
                name: "faction_rank_id",
                table: "character_faction_mappings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "faction_rank_id",
                table: "character_faction_mappings",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_character_faction_mappings_faction_rank_id",
                table: "character_faction_mappings",
                column: "faction_rank_id");

            migrationBuilder.AddForeignKey(
                name: "fk_character_faction_mappings_faction_ranks_faction_rank_id",
                table: "character_faction_mappings",
                column: "faction_rank_id",
                principalTable: "faction_ranks",
                principalColumn: "id");
        }
    }
}
