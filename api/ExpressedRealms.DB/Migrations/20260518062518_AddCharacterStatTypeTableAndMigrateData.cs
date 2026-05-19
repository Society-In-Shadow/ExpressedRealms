using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterStatTypeTableAndMigrateData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "character_stat_mapping",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    character_id = table.Column<int>(type: "integer", nullable: false),
                    stat_type_id = table.Column<byte>(type: "smallint", nullable: false),
                    stat_level_id = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character_stat_mapping", x => x.id);
                    table.ForeignKey(
                        name: "fk_character_stat_mapping_characters_character_id",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_character_stat_mapping_stat_levels_stat_level_id",
                        column: x => x.stat_level_id,
                        principalTable: "stat_levels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_character_stat_mapping_state_types_stat_type_id",
                        column: x => x.stat_type_id,
                        principalTable: "state_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_character_stat_mapping_character_id",
                table: "character_stat_mapping",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_stat_mapping_stat_level_id",
                table: "character_stat_mapping",
                column: "stat_level_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_stat_mapping_stat_type_id",
                table: "character_stat_mapping",
                column: "stat_type_id");

            migrationBuilder.Sql("""
                                 insert into public.character_stat_mapping (character_id, stat_type_id, stat_level_id)
                                 select v.character_id, v.stat_type_id, v.stat_level_id
                                 from public.characters
                                 cross join lateral (
                                 values 
                                 (id, 1, agility_id),
                                 (id, 2, constitution_id),
                                 (id, 3, dexterity_id),
                                 (id, 4, strength_id),
                                 (id, 5, intelligence_id),
                                 (id, 6, willpower_id)
                                 ) v(character_id, stat_type_id, stat_level_id)
                                 """);
            
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
            migrationBuilder.DropTable(
                name: "character_stat_mapping");
        }
    }
}
