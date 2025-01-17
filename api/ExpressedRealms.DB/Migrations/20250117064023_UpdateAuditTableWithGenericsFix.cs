using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditTableWithGenericsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSection_AuditTrail_AspNetUsers_UserId1",
                table: "ExpressionSection_AuditTrail");

            migrationBuilder.DropIndex(
                name: "IX_ExpressionSection_AuditTrail_UserId1",
                table: "ExpressionSection_AuditTrail");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ExpressionSection_AuditTrail");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ExpressionSection_AuditTrail",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_ExpressionSection_AuditTrail_UserId",
                table: "ExpressionSection_AuditTrail",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSection_AuditTrail_AspNetUsers_UserId",
                table: "ExpressionSection_AuditTrail",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionSection_AuditTrail_AspNetUsers_UserId",
                table: "ExpressionSection_AuditTrail");

            migrationBuilder.DropIndex(
                name: "IX_ExpressionSection_AuditTrail_UserId",
                table: "ExpressionSection_AuditTrail");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ExpressionSection_AuditTrail",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "ExpressionSection_AuditTrail",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ExpressionSection_AuditTrail_UserId1",
                table: "ExpressionSection_AuditTrail",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionSection_AuditTrail_AspNetUsers_UserId1",
                table: "ExpressionSection_AuditTrail",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
