using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToUserAuditTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "User_AuditTrail",
                type: "text",
                nullable: false,
                defaultValue: "");

            // Need to backport the changes, so will always have some bad data
            // I don't think it's that big of a deal, not many logs will be in here
            migrationBuilder.Sql(
                @"
                UPDATE public.""User_AuditTrail""
                SET ""UserId"" = ""ActorUserId"";
                ");
            
            migrationBuilder.CreateIndex(
                name: "IX_User_AuditTrail_UserId",
                table: "User_AuditTrail",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_AuditTrail_AspNetUsers_UserId",
                table: "User_AuditTrail",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_AuditTrail_AspNetUsers_UserId",
                table: "User_AuditTrail");

            migrationBuilder.DropIndex(
                name: "IX_User_AuditTrail_UserId",
                table: "User_AuditTrail");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "User_AuditTrail");
        }
    }
}
