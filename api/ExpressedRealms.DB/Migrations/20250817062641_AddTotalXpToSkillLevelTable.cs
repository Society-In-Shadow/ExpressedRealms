using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalXpToSkillLevelTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterSkillsMapping_SkillLevel_SkillLevelId",
                table: "CharacterSkillsMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillLevelBenefit_SkillLevel_SkillLevelId",
                table: "SkillLevelBenefit");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillLevelDescriptionMapping_SkillLevel_SkillLevelId",
                table: "SkillLevelDescriptionMapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SkillLevel",
                table: "SkillLevel");

            migrationBuilder.RenameTable(
                name: "SkillLevel",
                newName: "skill_level");

            migrationBuilder.RenameColumn(
                name: "XP",
                table: "skill_level",
                newName: "xp");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "skill_level",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "skill_level",
                newName: "level");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "skill_level",
                newName: "id");

            migrationBuilder.AddColumn<int>(
                name: "total_xp",
                table: "skill_level",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_skill_level",
                table: "skill_level",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterSkillsMapping_skill_level_SkillLevelId",
                table: "CharacterSkillsMapping",
                column: "SkillLevelId",
                principalTable: "skill_level",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillLevelBenefit_skill_level_SkillLevelId",
                table: "SkillLevelBenefit",
                column: "SkillLevelId",
                principalTable: "skill_level",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillLevelDescriptionMapping_skill_level_SkillLevelId",
                table: "SkillLevelDescriptionMapping",
                column: "SkillLevelId",
                principalTable: "skill_level",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterSkillsMapping_skill_level_SkillLevelId",
                table: "CharacterSkillsMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillLevelBenefit_skill_level_SkillLevelId",
                table: "SkillLevelBenefit");

            migrationBuilder.DropForeignKey(
                name: "FK_SkillLevelDescriptionMapping_skill_level_SkillLevelId",
                table: "SkillLevelDescriptionMapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_skill_level",
                table: "skill_level");

            migrationBuilder.DropColumn(
                name: "total_xp",
                table: "skill_level");

            migrationBuilder.RenameTable(
                name: "skill_level",
                newName: "SkillLevel");

            migrationBuilder.RenameColumn(
                name: "xp",
                table: "SkillLevel",
                newName: "XP");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "SkillLevel",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "level",
                table: "SkillLevel",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "SkillLevel",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SkillLevel",
                table: "SkillLevel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterSkillsMapping_SkillLevel_SkillLevelId",
                table: "CharacterSkillsMapping",
                column: "SkillLevelId",
                principalTable: "SkillLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillLevelBenefit_SkillLevel_SkillLevelId",
                table: "SkillLevelBenefit",
                column: "SkillLevelId",
                principalTable: "SkillLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SkillLevelDescriptionMapping_SkillLevel_SkillLevelId",
                table: "SkillLevelDescriptionMapping",
                column: "SkillLevelId",
                principalTable: "SkillLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
