using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCharacterTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mapping_assigned_xp_type_assigned_xp_type_id",
                table: "assigned_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mapping_characters_character_id",
                table: "assigned_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mapping_events_event_id",
                table: "assigned_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mapping_players_player_id",
                table: "assigned_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mapping_users_assigned_by_user_id",
                table: "assigned_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mapping_audit_trail_assigned_xp_mapping_assigne",
                table: "assigned_xp_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_type_audit_trail_assigned_xp_type_assigned_xp_t",
                table: "assigned_xp_type_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_character_xp_mapping_characters_character_id",
                table: "character_xp_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_character_xp_mapping_xp_section_types_xp_section_type_id",
                table: "character_xp_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "pk_xp_section_type",
                table: "xp_section_type");

            migrationBuilder.DropPrimaryKey(
                name: "pk_character_xp_mapping",
                table: "character_xp_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "pk_assigned_xp_type",
                table: "assigned_xp_type");

            migrationBuilder.DropPrimaryKey(
                name: "pk_assigned_xp_mapping",
                table: "assigned_xp_mapping");

            migrationBuilder.RenameTable(
                name: "xp_section_type",
                newName: "xp_section_types");

            migrationBuilder.RenameTable(
                name: "character_xp_mapping",
                newName: "character_xp_mappings");

            migrationBuilder.RenameTable(
                name: "assigned_xp_type",
                newName: "assigned_xp_types");

            migrationBuilder.RenameTable(
                name: "assigned_xp_mapping",
                newName: "assigned_xp_mappings");

            migrationBuilder.RenameColumn(
                name: "creation_cap",
                table: "xp_section_types",
                newName: "section_cap");

            migrationBuilder.RenameIndex(
                name: "ix_character_xp_mapping_xp_section_type_id",
                table: "character_xp_mappings",
                newName: "ix_character_xp_mappings_xp_section_type_id");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_mapping_player_id",
                table: "assigned_xp_mappings",
                newName: "ix_assigned_xp_mappings_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_mapping_event_id",
                table: "assigned_xp_mappings",
                newName: "ix_assigned_xp_mappings_event_id");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_mapping_character_id",
                table: "assigned_xp_mappings",
                newName: "ix_assigned_xp_mappings_character_id");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_mapping_assigned_xp_type_id",
                table: "assigned_xp_mappings",
                newName: "ix_assigned_xp_mappings_assigned_xp_type_id");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_mapping_assigned_by_user_id",
                table: "assigned_xp_mappings",
                newName: "ix_assigned_xp_mappings_assigned_by_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_xp_section_types",
                table: "xp_section_types",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_character_xp_mappings",
                table: "character_xp_mappings",
                columns: new[] { "character_id", "xp_section_type_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_assigned_xp_types",
                table: "assigned_xp_types",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_assigned_xp_mappings",
                table: "assigned_xp_mappings",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mapping_audit_trail_assigned_xp_mappings_assign",
                table: "assigned_xp_mapping_audit_trail",
                column: "assigned_xp_mapping_id",
                principalTable: "assigned_xp_mappings",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mappings_assigned_xp_types_assigned_xp_type_id",
                table: "assigned_xp_mappings",
                column: "assigned_xp_type_id",
                principalTable: "assigned_xp_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mappings_characters_character_id",
                table: "assigned_xp_mappings",
                column: "character_id",
                principalTable: "characters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mappings_events_event_id",
                table: "assigned_xp_mappings",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mappings_players_player_id",
                table: "assigned_xp_mappings",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mappings_users_assigned_by_user_id",
                table: "assigned_xp_mappings",
                column: "assigned_by_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_type_audit_trail_assigned_xp_types_assigned_xp_",
                table: "assigned_xp_type_audit_trail",
                column: "assigned_xp_type_id",
                principalTable: "assigned_xp_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_xp_mappings_characters_character_id",
                table: "character_xp_mappings",
                column: "character_id",
                principalTable: "characters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_xp_mappings_xp_section_types_xp_section_type_id",
                table: "character_xp_mappings",
                column: "xp_section_type_id",
                principalTable: "xp_section_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
            
            var assembly = Assembly.GetExecutingAssembly();

            using var stream = assembly.GetManifestResourceStream(
                "ExpressedRealms.DB.Scripts.CharacterXpView.sql"
            );

            if (stream == null)
                throw new InvalidOperationException("CharacterXpView.sql not found as embedded resource");

            using var reader = new StreamReader(stream);
            migrationBuilder.Sql(reader.ReadToEnd());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mapping_audit_trail_assigned_xp_mappings_assign",
                table: "assigned_xp_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mappings_assigned_xp_types_assigned_xp_type_id",
                table: "assigned_xp_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mappings_characters_character_id",
                table: "assigned_xp_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mappings_events_event_id",
                table: "assigned_xp_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mappings_players_player_id",
                table: "assigned_xp_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_mappings_users_assigned_by_user_id",
                table: "assigned_xp_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_assigned_xp_type_audit_trail_assigned_xp_types_assigned_xp_",
                table: "assigned_xp_type_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_character_xp_mappings_characters_character_id",
                table: "character_xp_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_character_xp_mappings_xp_section_types_xp_section_type_id",
                table: "character_xp_mappings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_xp_section_types",
                table: "xp_section_types");

            migrationBuilder.DropPrimaryKey(
                name: "pk_character_xp_mappings",
                table: "character_xp_mappings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_assigned_xp_types",
                table: "assigned_xp_types");

            migrationBuilder.DropPrimaryKey(
                name: "pk_assigned_xp_mappings",
                table: "assigned_xp_mappings");

            migrationBuilder.RenameTable(
                name: "xp_section_types",
                newName: "xp_section_type");

            migrationBuilder.RenameTable(
                name: "character_xp_mappings",
                newName: "character_xp_mapping");

            migrationBuilder.RenameTable(
                name: "assigned_xp_types",
                newName: "assigned_xp_type");

            migrationBuilder.RenameTable(
                name: "assigned_xp_mappings",
                newName: "assigned_xp_mapping");

            migrationBuilder.RenameColumn(
                name: "section_cap",
                table: "xp_section_type",
                newName: "creation_cap");

            migrationBuilder.RenameIndex(
                name: "ix_character_xp_mappings_xp_section_type_id",
                table: "character_xp_mapping",
                newName: "ix_character_xp_mapping_xp_section_type_id");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_mappings_player_id",
                table: "assigned_xp_mapping",
                newName: "ix_assigned_xp_mapping_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_mappings_event_id",
                table: "assigned_xp_mapping",
                newName: "ix_assigned_xp_mapping_event_id");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_mappings_character_id",
                table: "assigned_xp_mapping",
                newName: "ix_assigned_xp_mapping_character_id");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_mappings_assigned_xp_type_id",
                table: "assigned_xp_mapping",
                newName: "ix_assigned_xp_mapping_assigned_xp_type_id");

            migrationBuilder.RenameIndex(
                name: "ix_assigned_xp_mappings_assigned_by_user_id",
                table: "assigned_xp_mapping",
                newName: "ix_assigned_xp_mapping_assigned_by_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_xp_section_type",
                table: "xp_section_type",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_character_xp_mapping",
                table: "character_xp_mapping",
                columns: new[] { "character_id", "xp_section_type_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_assigned_xp_type",
                table: "assigned_xp_type",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_assigned_xp_mapping",
                table: "assigned_xp_mapping",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mapping_assigned_xp_type_assigned_xp_type_id",
                table: "assigned_xp_mapping",
                column: "assigned_xp_type_id",
                principalTable: "assigned_xp_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mapping_characters_character_id",
                table: "assigned_xp_mapping",
                column: "character_id",
                principalTable: "characters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mapping_events_event_id",
                table: "assigned_xp_mapping",
                column: "event_id",
                principalTable: "events",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mapping_players_player_id",
                table: "assigned_xp_mapping",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mapping_users_assigned_by_user_id",
                table: "assigned_xp_mapping",
                column: "assigned_by_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_mapping_audit_trail_assigned_xp_mapping_assigne",
                table: "assigned_xp_mapping_audit_trail",
                column: "assigned_xp_mapping_id",
                principalTable: "assigned_xp_mapping",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_assigned_xp_type_audit_trail_assigned_xp_type_assigned_xp_t",
                table: "assigned_xp_type_audit_trail",
                column: "assigned_xp_type_id",
                principalTable: "assigned_xp_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_xp_mapping_characters_character_id",
                table: "character_xp_mapping",
                column: "character_id",
                principalTable: "characters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_xp_mapping_xp_section_types_xp_section_type_id",
                table: "character_xp_mapping",
                column: "xp_section_type_id",
                principalTable: "xp_section_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
