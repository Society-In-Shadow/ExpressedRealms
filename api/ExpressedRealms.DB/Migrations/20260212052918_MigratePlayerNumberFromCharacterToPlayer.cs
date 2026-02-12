using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class MigratePlayerNumberFromCharacterToPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "player_number_sequence",
                startValue: 46L);

            migrationBuilder.AddColumn<int>(
                name: "player_number",
                table: "Players",
                type: "integer",
                nullable: true);
            
            migrationBuilder.Sql("""
                                 update public."Players" p
                                 set player_number = sub.player_number
                                 from (
                                     select 
                                         "PlayerId",
                                         min(player_number) as player_number
                                     from public."Characters"
                                     where player_number != 0
                                     group by "PlayerId"
                                 ) sub
                                 where p."Id" = sub."PlayerId";
                                 """);

            migrationBuilder.CreateIndex(
                name: "IX_Players_player_number",
                table: "Players",
                column: "player_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Players_player_number",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "player_number",
                table: "Players");

            migrationBuilder.DropSequence(
                name: "player_number_sequence");
        }
    }
}
