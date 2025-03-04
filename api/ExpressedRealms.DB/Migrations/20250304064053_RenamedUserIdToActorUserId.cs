using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class RenamedUserIdToActorUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expression_AuditTrail_AspNetUsers_UserId",
                table: "Expression_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSection_AuditTrail_AspNetUsers_UserId",
                table: "ExpressionSection_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_AuditTrail_AspNetUsers_UserId",
                table: "Player_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_User_AuditTrail_AspNetUsers_UserId",
                table: "User_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_AuditTrail_AspNetUsers_UserId",
                table: "UserRoles_AuditTrail");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserRoles_AuditTrail",
                newName: "ActorUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_AuditTrail_UserId",
                table: "UserRoles_AuditTrail",
                newName: "IX_UserRoles_AuditTrail_ActorUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "User_AuditTrail",
                newName: "ActorUserId");

            migrationBuilder.RenameIndex(
                name: "IX_User_AuditTrail_UserId",
                table: "User_AuditTrail",
                newName: "IX_User_AuditTrail_ActorUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Player_AuditTrail",
                newName: "ActorUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Player_AuditTrail_UserId",
                table: "Player_AuditTrail",
                newName: "IX_Player_AuditTrail_ActorUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ExpressionSection_AuditTrail",
                newName: "ActorUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpressionSection_AuditTrail_UserId",
                table: "ExpressionSection_AuditTrail",
                newName: "IX_ExpressionSection_AuditTrail_ActorUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Expression_AuditTrail",
                newName: "ActorUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Expression_AuditTrail_UserId",
                table: "Expression_AuditTrail",
                newName: "IX_Expression_AuditTrail_ActorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expression_AuditTrail_AspNetUsers_ActorUserId",
                table: "Expression_AuditTrail",
                column: "ActorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSection_AuditTrail_AspNetUsers_ActorUserId",
                table: "ExpressionSection_AuditTrail",
                column: "ActorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_AuditTrail_AspNetUsers_ActorUserId",
                table: "Player_AuditTrail",
                column: "ActorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_AuditTrail_AspNetUsers_ActorUserId",
                table: "User_AuditTrail",
                column: "ActorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_AuditTrail_AspNetUsers_ActorUserId",
                table: "UserRoles_AuditTrail",
                column: "ActorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expression_AuditTrail_AspNetUsers_ActorUserId",
                table: "Expression_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSection_AuditTrail_AspNetUsers_ActorUserId",
                table: "ExpressionSection_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_AuditTrail_AspNetUsers_ActorUserId",
                table: "Player_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_User_AuditTrail_AspNetUsers_ActorUserId",
                table: "User_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_AuditTrail_AspNetUsers_ActorUserId",
                table: "UserRoles_AuditTrail");

            migrationBuilder.RenameColumn(
                name: "ActorUserId",
                table: "UserRoles_AuditTrail",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_AuditTrail_ActorUserId",
                table: "UserRoles_AuditTrail",
                newName: "IX_UserRoles_AuditTrail_UserId");

            migrationBuilder.RenameColumn(
                name: "ActorUserId",
                table: "User_AuditTrail",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_User_AuditTrail_ActorUserId",
                table: "User_AuditTrail",
                newName: "IX_User_AuditTrail_UserId");

            migrationBuilder.RenameColumn(
                name: "ActorUserId",
                table: "Player_AuditTrail",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Player_AuditTrail_ActorUserId",
                table: "Player_AuditTrail",
                newName: "IX_Player_AuditTrail_UserId");

            migrationBuilder.RenameColumn(
                name: "ActorUserId",
                table: "ExpressionSection_AuditTrail",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpressionSection_AuditTrail_ActorUserId",
                table: "ExpressionSection_AuditTrail",
                newName: "IX_ExpressionSection_AuditTrail_UserId");

            migrationBuilder.RenameColumn(
                name: "ActorUserId",
                table: "Expression_AuditTrail",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Expression_AuditTrail_ActorUserId",
                table: "Expression_AuditTrail",
                newName: "IX_Expression_AuditTrail_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expression_AuditTrail_AspNetUsers_UserId",
                table: "Expression_AuditTrail",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSection_AuditTrail_AspNetUsers_UserId",
                table: "ExpressionSection_AuditTrail",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_AuditTrail_AspNetUsers_UserId",
                table: "Player_AuditTrail",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_AuditTrail_AspNetUsers_UserId",
                table: "User_AuditTrail",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_AuditTrail_AspNetUsers_UserId",
                table: "UserRoles_AuditTrail",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
