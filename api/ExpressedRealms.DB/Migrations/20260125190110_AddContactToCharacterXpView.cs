using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddContactToCharacterXpView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using var stream = assembly.GetManifestResourceStream(
                "ExpressedRealms.DB.Scripts.CharacterXpView.sql"
            );

            if (stream == null)
                throw new InvalidOperationException("CharacterXpView.sql not found as embedded resource");

            using var reader = new StreamReader(stream);
            migrationBuilder.Sql(reader.ReadToEnd());

            migrationBuilder.Sql("""
                                 insert into public.xp_section_type (id, name, creation_cap)
                                 values (8, 'Contact XP', 0)
                                 """);

            migrationBuilder.Sql("""
                                 insert into public.character_xp_mapping (character_id, xp_section_type_id, section_cap, spent_xp, discretion_xp, total_character_creation_xp, level_xp)
                                 select "Id", 8, 0, 0, 0, 0, 0  from public."Characters"
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
