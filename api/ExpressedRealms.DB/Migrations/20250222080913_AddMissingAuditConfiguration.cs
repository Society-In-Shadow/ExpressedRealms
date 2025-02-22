using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingAuditConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionAuditTrail_AspNetUsers_UserId",
                table: "ExpressionAuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpressionAuditTrail_Expressions_ExpressionId",
                table: "ExpressionAuditTrail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpressionAuditTrail",
                table: "ExpressionAuditTrail");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "ExpressionAuditTrail",
                newName: "Expression_AuditTrail");

            migrationBuilder.RenameIndex(
                name: "IX_ExpressionAuditTrail_UserId",
                table: "Expression_AuditTrail",
                newName: "IX_Expression_AuditTrail_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpressionAuditTrail_ExpressionId",
                table: "Expression_AuditTrail",
                newName: "IX_Expression_AuditTrail_ExpressionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expression_AuditTrail",
                table: "Expression_AuditTrail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expression_AuditTrail_AspNetUsers_UserId",
                table: "Expression_AuditTrail",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Expression_AuditTrail_Expressions_ExpressionId",
                table: "Expression_AuditTrail",
                column: "ExpressionId",
                principalTable: "Expressions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expression_AuditTrail_AspNetUsers_UserId",
                table: "Expression_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_Expression_AuditTrail_Expressions_ExpressionId",
                table: "Expression_AuditTrail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expression_AuditTrail",
                table: "Expression_AuditTrail");

            migrationBuilder.RenameTable(
                name: "Expression_AuditTrail",
                newName: "ExpressionAuditTrail");

            migrationBuilder.RenameIndex(
                name: "IX_Expression_AuditTrail_UserId",
                table: "ExpressionAuditTrail",
                newName: "IX_ExpressionAuditTrail_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Expression_AuditTrail_ExpressionId",
                table: "ExpressionAuditTrail",
                newName: "IX_ExpressionAuditTrail_ExpressionId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpressionAuditTrail",
                table: "ExpressionAuditTrail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionAuditTrail_AspNetUsers_UserId",
                table: "ExpressionAuditTrail",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpressionAuditTrail_Expressions_ExpressionId",
                table: "ExpressionAuditTrail",
                column: "ExpressionId",
                principalTable: "Expressions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
