using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddExtraMortisFlagToCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "extra_mortis",
                table: "characters",
                type: "boolean",
                nullable: false,
                defaultValue: false);
            
            migrationBuilder.ExecuteEmbeddedSqlScript(MigrationHelpers.EmbeddedFiles.CopyCharacterToPlayerProc);
            migrationBuilder.ExecuteEmbeddedSqlScript(MigrationHelpers.EmbeddedFiles.CharacterXpView);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "extra_mortis",
                table: "characters");
        }
    }
}
