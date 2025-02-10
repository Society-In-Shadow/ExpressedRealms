using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddSkillMappingsToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            INSERT INTO public.""CharacterSkillsMapping"" (""CharacterId"", ""SkillTypeId"", ""SkillLevelId"") 
            SELECT 
                c.""Id"" AS character_id,
                s.""Id"" AS skill_type_id,
                1 AS skill_level_id
            FROM public.""Characters"" c
            CROSS JOIN public.""SkillType"" s
            WHERE NOT EXISTS (
                SELECT 1 
                FROM public.""CharacterSkillsMapping"" cm
                WHERE cm.""CharacterId"" = c.""Id"" 
                  AND cm.""SkillTypeId"" = s.""Id""
            );
        ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
