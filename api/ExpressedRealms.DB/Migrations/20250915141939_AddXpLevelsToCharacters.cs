using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddXpLevelsToCharacters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "xp_section_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    creation_cap = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_xp_section_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "character_xp_mapping",
                columns: table => new
                {
                    character_id = table.Column<int>(type: "integer", nullable: false),
                    xp_section_type_id = table.Column<int>(type: "integer", nullable: false),
                    section_cap = table.Column<int>(type: "integer", nullable: false),
                    spent_xp = table.Column<int>(type: "integer", nullable: false),
                    discretion_xp = table.Column<int>(type: "integer", nullable: false),
                    total_character_creation_xp = table.Column<int>(type: "integer", nullable: false),
                    level_xp = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_character_xp_mapping", x => new { x.character_id, x.xp_section_type_id });
                    table.ForeignKey(
                        name: "FK_character_xp_mapping_Characters_character_id",
                        column: x => x.character_id,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_character_xp_mapping_xp_section_type_xp_section_type_id",
                        column: x => x.xp_section_type_id,
                        principalTable: "xp_section_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_character_xp_mapping_xp_section_type_id",
                table: "character_xp_mapping",
                column: "xp_section_type_id");


            migrationBuilder.Sql(@"
                insert into public.xp_section_type (id, name, creation_cap)
                values (1, 'Advantage / Disadvantage XP', 8),
                (2, 'Knowledge XP', 7),
                (3, 'Power XP', 20),
                (4, 'Skills XP', 28),
                (5, 'Stats XP', 72),
                (6, 'Discretionary', 16);");

            migrationBuilder.Sql(@"
with calc as (

	-- Blessing XP
	select ""Id"" character_id, sum(blessing_level.xp_cost) xp_total, 1 section_Type_id from public.""Characters""
	join public.character_blessing_mapping on ""Characters"".""Id"" = character_blessing_mapping.character_Id
	join public.blessing on blessing.id = character_blessing_mapping.blessing_id
	join public.blessing_level on blessing_level.id = character_blessing_mapping.blessing_level_id
	where blessing.is_deleted = false and character_blessing_Mapping.is_deleted = false
	group by ""Characters"".""Id""

	union -- Knowledges XP
	select ""Id"" character_id, sum(knowledge_education_level.total_general_xp_cost) + count(character_knowledge_specialization.id) * 2 xp_total, 2 section_type_id from public.""Characters""
	join public.character_knowledge_mapping on ""Characters"".""Id"" = character_knowledge_mapping.character_Id
	left join public.character_knowledge_specialization on character_knowledge_mapping.id = character_knowledge_specialization.knowledge_mapping_id
	join public.knowledge_education_level on character_knowledge_mapping.knowledge_level_id = knowledge_education_level.id
	where character_knowledge_mapping.is_deleted = false and character_knowledge_specialization.is_deleted = false
	group by ""Characters"".""Id""
	
	union -- Raw Power XP
	select ""Id"" character_id, sum(power_level.xp) xp_total, 3 section_type_id from public.""Characters""
	join public.character_power_mapping on ""Characters"".""Id"" = character_power_mapping.character_Id
	join public.power_level on power_level.id = character_power_mapping.power_level_id
	where character_power_mapping.is_deleted = false
	group by ""Characters"".""Id""

	union -- Raw Skills XP
	select ""Characters"".""Id"" character_id, sum(skill_level.total_xp) total_xp, 4 section_type_id from public.""Characters""
	join public.""CharacterSkillsMapping"" on ""Characters"".""Id"" = ""CharacterSkillsMapping"".""CharacterId""
	join public.skill_level on ""CharacterSkillsMapping"".""SkillLevelId"" = skill_level.id
	group by ""Characters"".""Id""
	
	union -- Raw Stat XP
	select ""Characters"".""Id"" character_id, agility.""TotalXPCost"" + con.""TotalXPCost"" + dex.""TotalXPCost"" + stre.""TotalXPCost"" + inti.""TotalXPCost"" + wil.""TotalXPCost"" xp_total, 5 section_type_id from public.""Characters""
	join public.""StatLevels"" agility on ""Characters"".""AgilityId"" = agility.""Id""
	join public.""StatLevels"" con on ""Characters"".""ConstitutionId"" = con.""Id""
	join public.""StatLevels"" dex on ""Characters"".""DexterityId"" = dex.""Id""
	join public.""StatLevels"" stre on ""Characters"".""StrengthId"" = stre.""Id""
	join public.""StatLevels"" inti on ""Characters"".""IntelligenceId"" = inti.""Id""
	join public.""StatLevels"" wil on ""Characters"".""WillpowerId"" = wil.""Id""
)
insert into public.character_xp_mapping (character_id, xp_section_type_id, section_cap, spent_xp, discretion_xp, total_character_creation_xp, level_xp)
select 
	""Characters"".""Id"", 
	xp_section_Type.id,
	xp_section_type.creation_cap, 
	COALESCE(calc.xp_total, 0) spent_xp,
	COALESCE(calc.xp_total - xp_section_type.creation_cap, 0) discretion_xp,
	0 total_character_creation_xp, 
	0 level_xp
from public.""Characters""
cross join public.xp_section_type
left join calc on calc.character_id = ""Characters"".""Id"" and calc.section_type_id = xp_section_type.id
order by ""Characters"".""Id""
");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_xp_mapping");

            migrationBuilder.DropTable(
                name: "xp_section_type");
        }
    }
}
