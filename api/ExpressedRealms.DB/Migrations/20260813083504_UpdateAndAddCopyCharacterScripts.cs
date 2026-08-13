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
            migrationBuilder.ExecuteEmbeddedSqlScript("ExpressedRealms.DB.Scripts.CopyExpression.sql");
            migrationBuilder.ExecuteEmbeddedSqlScript("ExpressedRealms.DB.Scripts.CopyCharacterToPlayerProc.sql");
            migrationBuilder.ExecuteEmbeddedSqlScript("ExpressedRealms.DB.Scripts.CopyModifiers.sql");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
