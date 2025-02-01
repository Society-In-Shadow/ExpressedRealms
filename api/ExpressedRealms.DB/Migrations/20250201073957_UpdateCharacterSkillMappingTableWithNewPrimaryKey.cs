using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCharacterSkillMappingTableWithNewPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CharacterSkillsMapping",
                table: "CharacterSkillsMapping");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharacterSkillsMapping",
                table: "CharacterSkillsMapping",
                columns: new[] { "CharacterId", "SkillTypeId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CharacterSkillsMapping",
                table: "CharacterSkillsMapping");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharacterSkillsMapping",
                table: "CharacterSkillsMapping",
                columns: new[] { "CharacterId", "SkillLevelId", "SkillTypeId" });
        }
    }
}
