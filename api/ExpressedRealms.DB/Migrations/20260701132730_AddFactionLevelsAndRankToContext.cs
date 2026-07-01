using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddFactionLevelsAndRankToContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_faction_level_faction_rank_faction_rank_id",
                table: "faction_level");

            migrationBuilder.DropForeignKey(
                name: "fk_faction_level_factions_faction_id",
                table: "faction_level");

            migrationBuilder.DropForeignKey(
                name: "fk_faction_level_knowledge_education_levels_knowledge_level_id",
                table: "faction_level");

            migrationBuilder.DropForeignKey(
                name: "fk_faction_level_knowledges_knowledge_id",
                table: "faction_level");

            migrationBuilder.DropForeignKey(
                name: "fk_faction_level_audit_trail_faction_level_faction_level_id",
                table: "faction_level_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_faction_rank",
                table: "faction_rank");

            migrationBuilder.DropPrimaryKey(
                name: "pk_faction_level",
                table: "faction_level");

            migrationBuilder.RenameTable(
                name: "faction_rank",
                newName: "faction_ranks");

            migrationBuilder.RenameTable(
                name: "faction_level",
                newName: "faction_levels");

            migrationBuilder.RenameIndex(
                name: "ix_faction_level_knowledge_level_id",
                table: "faction_levels",
                newName: "ix_faction_levels_knowledge_level_id");

            migrationBuilder.RenameIndex(
                name: "ix_faction_level_knowledge_id",
                table: "faction_levels",
                newName: "ix_faction_levels_knowledge_id");

            migrationBuilder.RenameIndex(
                name: "ix_faction_level_faction_rank_id",
                table: "faction_levels",
                newName: "ix_faction_levels_faction_rank_id");

            migrationBuilder.RenameIndex(
                name: "ix_faction_level_faction_id",
                table: "faction_levels",
                newName: "ix_faction_levels_faction_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_faction_ranks",
                table: "faction_ranks",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_faction_levels",
                table: "faction_levels",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_faction_level_audit_trail_faction_levels_faction_level_id",
                table: "faction_level_audit_trail",
                column: "faction_level_id",
                principalTable: "faction_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_faction_levels_faction_ranks_faction_rank_id",
                table: "faction_levels",
                column: "faction_rank_id",
                principalTable: "faction_ranks",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_faction_levels_factions_faction_id",
                table: "faction_levels",
                column: "faction_id",
                principalTable: "factions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_faction_levels_knowledge_education_levels_knowledge_level_id",
                table: "faction_levels",
                column: "knowledge_level_id",
                principalTable: "knowledge_education_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_faction_levels_knowledges_knowledge_id",
                table: "faction_levels",
                column: "knowledge_id",
                principalTable: "knowledges",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_faction_level_audit_trail_faction_levels_faction_level_id",
                table: "faction_level_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_faction_levels_faction_ranks_faction_rank_id",
                table: "faction_levels");

            migrationBuilder.DropForeignKey(
                name: "fk_faction_levels_factions_faction_id",
                table: "faction_levels");

            migrationBuilder.DropForeignKey(
                name: "fk_faction_levels_knowledge_education_levels_knowledge_level_id",
                table: "faction_levels");

            migrationBuilder.DropForeignKey(
                name: "fk_faction_levels_knowledges_knowledge_id",
                table: "faction_levels");

            migrationBuilder.DropPrimaryKey(
                name: "pk_faction_ranks",
                table: "faction_ranks");

            migrationBuilder.DropPrimaryKey(
                name: "pk_faction_levels",
                table: "faction_levels");

            migrationBuilder.RenameTable(
                name: "faction_ranks",
                newName: "faction_rank");

            migrationBuilder.RenameTable(
                name: "faction_levels",
                newName: "faction_level");

            migrationBuilder.RenameIndex(
                name: "ix_faction_levels_knowledge_level_id",
                table: "faction_level",
                newName: "ix_faction_level_knowledge_level_id");

            migrationBuilder.RenameIndex(
                name: "ix_faction_levels_knowledge_id",
                table: "faction_level",
                newName: "ix_faction_level_knowledge_id");

            migrationBuilder.RenameIndex(
                name: "ix_faction_levels_faction_rank_id",
                table: "faction_level",
                newName: "ix_faction_level_faction_rank_id");

            migrationBuilder.RenameIndex(
                name: "ix_faction_levels_faction_id",
                table: "faction_level",
                newName: "ix_faction_level_faction_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_faction_rank",
                table: "faction_rank",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_faction_level",
                table: "faction_level",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_faction_level_faction_rank_faction_rank_id",
                table: "faction_level",
                column: "faction_rank_id",
                principalTable: "faction_rank",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_faction_level_factions_faction_id",
                table: "faction_level",
                column: "faction_id",
                principalTable: "factions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_faction_level_knowledge_education_levels_knowledge_level_id",
                table: "faction_level",
                column: "knowledge_level_id",
                principalTable: "knowledge_education_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_faction_level_knowledges_knowledge_id",
                table: "faction_level",
                column: "knowledge_id",
                principalTable: "knowledges",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_faction_level_audit_trail_faction_level_faction_level_id",
                table: "faction_level_audit_trail",
                column: "faction_level_id",
                principalTable: "faction_level",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
