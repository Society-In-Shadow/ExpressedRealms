using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerAuditTableConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerAuditTrail_AspNetUsers_UserId",
                table: "PlayerAuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerAuditTrail_Players_PlayerId",
                table: "PlayerAuditTrail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerAuditTrail",
                table: "PlayerAuditTrail");

            migrationBuilder.RenameTable(
                name: "PlayerAuditTrail",
                newName: "Player_AuditTrail");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerAuditTrail_UserId",
                table: "Player_AuditTrail",
                newName: "IX_Player_AuditTrail_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerAuditTrail_PlayerId",
                table: "Player_AuditTrail",
                newName: "IX_Player_AuditTrail_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Player_AuditTrail",
                table: "Player_AuditTrail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_AuditTrail_AspNetUsers_UserId",
                table: "Player_AuditTrail",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_AuditTrail_Players_PlayerId",
                table: "Player_AuditTrail",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_AuditTrail_AspNetUsers_UserId",
                table: "Player_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_AuditTrail_Players_PlayerId",
                table: "Player_AuditTrail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Player_AuditTrail",
                table: "Player_AuditTrail");

            migrationBuilder.RenameTable(
                name: "Player_AuditTrail",
                newName: "PlayerAuditTrail");

            migrationBuilder.RenameIndex(
                name: "IX_Player_AuditTrail_UserId",
                table: "PlayerAuditTrail",
                newName: "IX_PlayerAuditTrail_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Player_AuditTrail_PlayerId",
                table: "PlayerAuditTrail",
                newName: "IX_PlayerAuditTrail_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerAuditTrail",
                table: "PlayerAuditTrail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerAuditTrail_AspNetUsers_UserId",
                table: "PlayerAuditTrail",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerAuditTrail_Players_PlayerId",
                table: "PlayerAuditTrail",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
