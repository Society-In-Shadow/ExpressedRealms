using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyBetweenPlayerAuditAndPlayerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_AuditTrail_Players_PlayerId",
                table: "Player_AuditTrail");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_AuditTrail_Players_PlayerId",
                table: "Player_AuditTrail",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_AuditTrail_Players_PlayerId",
                table: "Player_AuditTrail");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_AuditTrail_Players_PlayerId",
                table: "Player_AuditTrail",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
