using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModifierTableWithNoDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkillLevelBenefit_ModifierType_ModifierTypeId",
                table: "SkillLevelBenefit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModifierType",
                table: "ModifierType");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ModifierType");

            migrationBuilder.RenameTable(
                name: "ModifierType",
                newName: "modifier_type");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "modifier_type",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "modifier_type",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "modifier_type",
                type: "character varying(75)",
                maxLength: 75,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_modifier_type",
                table: "modifier_type",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillLevelBenefit_modifier_type_ModifierTypeId",
                table: "SkillLevelBenefit",
                column: "ModifierTypeId",
                principalTable: "modifier_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SkillLevelBenefit_modifier_type_ModifierTypeId",
                table: "SkillLevelBenefit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_modifier_type",
                table: "modifier_type");

            migrationBuilder.RenameTable(
                name: "modifier_type",
                newName: "ModifierType");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "ModifierType",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ModifierType",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ModifierType",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(75)",
                oldMaxLength: 75);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ModifierType",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModifierType",
                table: "ModifierType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SkillLevelBenefit_ModifierType_ModifierTypeId",
                table: "SkillLevelBenefit",
                column: "ModifierTypeId",
                principalTable: "ModifierType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
