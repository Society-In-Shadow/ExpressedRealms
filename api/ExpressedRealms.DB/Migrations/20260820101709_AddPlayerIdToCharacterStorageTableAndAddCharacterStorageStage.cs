using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerIdToCharacterStorageTableAndAddCharacterStorageStage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_character_storage_infos_players_collector_user_id",
                table: "character_storage_infos");

            migrationBuilder.DropForeignKey(
                name: "fk_character_storage_infos_players_sign_off_user_id",
                table: "character_storage_infos");

            migrationBuilder.RenameColumn(
                name: "sign_off_user_id",
                table: "character_storage_infos",
                newName: "sign_off_player_id");

            migrationBuilder.RenameColumn(
                name: "collector_user_id",
                table: "character_storage_infos",
                newName: "player_id");

            migrationBuilder.RenameIndex(
                name: "ix_character_storage_infos_sign_off_user_id",
                table: "character_storage_infos",
                newName: "ix_character_storage_infos_sign_off_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_character_storage_infos_collector_user_id",
                table: "character_storage_infos",
                newName: "ix_character_storage_infos_player_id");

            migrationBuilder.AddColumn<Guid>(
                name: "collector_player_id",
                table: "character_storage_infos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "checkin_stages",
                columns: new[] { "id", "description", "name" },
                values: new object[] { 13, "Players have the ability to pay for character storage, this step gets that sorted out.", "Character Storage Question" });

            migrationBuilder.CreateIndex(
                name: "ix_character_storage_infos_collector_player_id",
                table: "character_storage_infos",
                column: "collector_player_id");

            migrationBuilder.AddForeignKey(
                name: "fk_character_storage_infos_players_collector_player_id",
                table: "character_storage_infos",
                column: "collector_player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_storage_infos_players_player_id",
                table: "character_storage_infos",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_storage_infos_players_sign_off_player_id",
                table: "character_storage_infos",
                column: "sign_off_player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_character_storage_infos_players_collector_player_id",
                table: "character_storage_infos");

            migrationBuilder.DropForeignKey(
                name: "fk_character_storage_infos_players_player_id",
                table: "character_storage_infos");

            migrationBuilder.DropForeignKey(
                name: "fk_character_storage_infos_players_sign_off_player_id",
                table: "character_storage_infos");

            migrationBuilder.DropIndex(
                name: "ix_character_storage_infos_collector_player_id",
                table: "character_storage_infos");

            migrationBuilder.DeleteData(
                table: "checkin_stages",
                keyColumn: "id",
                keyValue: 13);

            migrationBuilder.DropColumn(
                name: "collector_player_id",
                table: "character_storage_infos");

            migrationBuilder.RenameColumn(
                name: "sign_off_player_id",
                table: "character_storage_infos",
                newName: "sign_off_user_id");

            migrationBuilder.RenameColumn(
                name: "player_id",
                table: "character_storage_infos",
                newName: "collector_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_character_storage_infos_sign_off_player_id",
                table: "character_storage_infos",
                newName: "ix_character_storage_infos_sign_off_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_character_storage_infos_player_id",
                table: "character_storage_infos",
                newName: "ix_character_storage_infos_collector_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_character_storage_infos_players_collector_user_id",
                table: "character_storage_infos",
                column: "collector_user_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_character_storage_infos_players_sign_off_user_id",
                table: "character_storage_infos",
                column: "sign_off_user_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
