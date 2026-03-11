using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerAgeGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "age_group_id",
                table: "players",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "has_consent_form",
                table: "players",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "player_age_group",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_player_age_group", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "player_age_group",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Child (<13)" },
                    { 2, "Teen (13-17)" },
                    { 3, "Adult (18+)" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_players_age_group_id",
                table: "players",
                column: "age_group_id");

            migrationBuilder.AddForeignKey(
                name: "fk_players_player_age_group_age_group_id",
                table: "players",
                column: "age_group_id",
                principalTable: "player_age_group",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_players_player_age_group_age_group_id",
                table: "players");

            migrationBuilder.DropTable(
                name: "player_age_group");

            migrationBuilder.DropIndex(
                name: "ix_players_age_group_id",
                table: "players");

            migrationBuilder.DropColumn(
                name: "age_group_id",
                table: "players");

            migrationBuilder.DropColumn(
                name: "has_consent_form",
                table: "players");
        }
    }
}
