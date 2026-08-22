using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSignoffUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_character_storage_infos_players_sign_off_player_id",
                table: "character_storage_infos");

            migrationBuilder.DropIndex(
                name: "ix_character_storage_infos_sign_off_player_id",
                table: "character_storage_infos");

            migrationBuilder.DropColumn(
                name: "sign_off_player_id",
                table: "character_storage_infos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "sign_off_player_id",
                table: "character_storage_infos",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_character_storage_infos_sign_off_player_id",
                table: "character_storage_infos",
                column: "sign_off_player_id");

            migrationBuilder.AddForeignKey(
                name: "fk_character_storage_infos_players_sign_off_player_id",
                table: "character_storage_infos",
                column: "sign_off_player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
