using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddDbContextForCharacterStatMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_character_stat_mapping_characters_character_id",
                table: "character_stat_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_character_stat_mapping_stat_levels_stat_level_id",
                table: "character_stat_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_character_stat_mapping_state_types_stat_type_id",
                table: "character_stat_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "pk_character_stat_mapping",
                table: "character_stat_mapping");

            migrationBuilder.RenameTable(
                name: "character_stat_mapping",
                newName: "character_stat_mappings");

            migrationBuilder.RenameIndex(
                name: "ix_character_stat_mapping_stat_type_id",
                table: "character_stat_mappings",
                newName: "ix_character_stat_mappings_stat_type_id");

            migrationBuilder.RenameIndex(
                name: "ix_character_stat_mapping_stat_level_id",
                table: "character_stat_mappings",
                newName: "ix_character_stat_mappings_stat_level_id");

            migrationBuilder.RenameIndex(
                name: "ix_character_stat_mapping_character_id",
                table: "character_stat_mappings",
                newName: "ix_character_stat_mappings_character_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_character_stat_mappings",
                table: "character_stat_mappings",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_character_stat_mappings_characters_character_id",
                table: "character_stat_mappings",
                column: "character_id",
                principalTable: "characters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_stat_mappings_stat_levels_stat_level_id",
                table: "character_stat_mappings",
                column: "stat_level_id",
                principalTable: "stat_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_stat_mappings_state_types_stat_type_id",
                table: "character_stat_mappings",
                column: "stat_type_id",
                principalTable: "state_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_character_stat_mappings_characters_character_id",
                table: "character_stat_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_character_stat_mappings_stat_levels_stat_level_id",
                table: "character_stat_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_character_stat_mappings_state_types_stat_type_id",
                table: "character_stat_mappings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_character_stat_mappings",
                table: "character_stat_mappings");

            migrationBuilder.RenameTable(
                name: "character_stat_mappings",
                newName: "character_stat_mapping");

            migrationBuilder.RenameIndex(
                name: "ix_character_stat_mappings_stat_type_id",
                table: "character_stat_mapping",
                newName: "ix_character_stat_mapping_stat_type_id");

            migrationBuilder.RenameIndex(
                name: "ix_character_stat_mappings_stat_level_id",
                table: "character_stat_mapping",
                newName: "ix_character_stat_mapping_stat_level_id");

            migrationBuilder.RenameIndex(
                name: "ix_character_stat_mappings_character_id",
                table: "character_stat_mapping",
                newName: "ix_character_stat_mapping_character_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_character_stat_mapping",
                table: "character_stat_mapping",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_character_stat_mapping_characters_character_id",
                table: "character_stat_mapping",
                column: "character_id",
                principalTable: "characters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_stat_mapping_stat_levels_stat_level_id",
                table: "character_stat_mapping",
                column: "stat_level_id",
                principalTable: "stat_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_stat_mapping_state_types_stat_type_id",
                table: "character_stat_mapping",
                column: "stat_type_id",
                principalTable: "state_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
