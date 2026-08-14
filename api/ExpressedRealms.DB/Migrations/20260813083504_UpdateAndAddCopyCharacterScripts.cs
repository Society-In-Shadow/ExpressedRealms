using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAndAddCopyCharacterScripts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE copy_character_to_player_proc");
            migrationBuilder.ExecuteEmbeddedSqlScript(MigrationHelpers.EmbeddedFiles.CopyExpression);
            migrationBuilder.ExecuteEmbeddedSqlScript(MigrationHelpers.EmbeddedFiles.CopyCharacterToPlayerProc);
            migrationBuilder.ExecuteEmbeddedSqlScript(MigrationHelpers.EmbeddedFiles.CopyModifiers);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
