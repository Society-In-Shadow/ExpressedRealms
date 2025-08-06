using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class FixKnowledgedMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_character_knowledge_mapping_knowledge_knowledge_level_id",
                table: "character_knowledge_mapping");

            migrationBuilder.CreateIndex(
                name: "IX_character_knowledge_mapping_knowledge_id",
                table: "character_knowledge_mapping",
                column: "knowledge_id");

            migrationBuilder.AddForeignKey(
                name: "FK_character_knowledge_mapping_knowledge_knowledge_id",
                table: "character_knowledge_mapping",
                column: "knowledge_id",
                principalTable: "knowledge",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_character_knowledge_mapping_knowledge_knowledge_id",
                table: "character_knowledge_mapping");

            migrationBuilder.DropIndex(
                name: "IX_character_knowledge_mapping_knowledge_id",
                table: "character_knowledge_mapping");

            migrationBuilder.AddForeignKey(
                name: "FK_character_knowledge_mapping_knowledge_knowledge_level_id",
                table: "character_knowledge_mapping",
                column: "knowledge_level_id",
                principalTable: "knowledge",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
