using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class CorrectlyAddPathAuditTrails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PowerPathAuditTrail_AspNetUsers_ActorUserId",
                table: "PowerPathAuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_PowerPathAuditTrail_Expressions_ExpressionId",
                table: "PowerPathAuditTrail");

            migrationBuilder.DropForeignKey(
                name: "FK_PowerPathAuditTrail_power_path_PowerPathId",
                table: "PowerPathAuditTrail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PowerPathAuditTrail",
                table: "PowerPathAuditTrail");

            migrationBuilder.RenameTable(
                name: "PowerPathAuditTrail",
                newName: "power_path_audit_trail");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "power_path_audit_trail",
                newName: "timestamp");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "power_path_audit_trail",
                newName: "action");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "power_path_audit_trail",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "PowerPathId",
                table: "power_path_audit_trail",
                newName: "power_path_id");

            migrationBuilder.RenameColumn(
                name: "ExpressionId",
                table: "power_path_audit_trail",
                newName: "expression_id");

            migrationBuilder.RenameColumn(
                name: "ChangedProperties",
                table: "power_path_audit_trail",
                newName: "changed_properties");

            migrationBuilder.RenameColumn(
                name: "ActorUserId",
                table: "power_path_audit_trail",
                newName: "actor_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_PowerPathAuditTrail_PowerPathId",
                table: "power_path_audit_trail",
                newName: "IX_power_path_audit_trail_power_path_id");

            migrationBuilder.RenameIndex(
                name: "IX_PowerPathAuditTrail_ExpressionId",
                table: "power_path_audit_trail",
                newName: "IX_power_path_audit_trail_expression_id");

            migrationBuilder.RenameIndex(
                name: "IX_PowerPathAuditTrail_ActorUserId",
                table: "power_path_audit_trail",
                newName: "IX_power_path_audit_trail_actor_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_power_path_audit_trail",
                table: "power_path_audit_trail",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_power_path_audit_trail_AspNetUsers_actor_user_id",
                table: "power_path_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_path_audit_trail_Expressions_expression_id",
                table: "power_path_audit_trail",
                column: "expression_id",
                principalTable: "Expressions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_power_path_audit_trail_power_path_power_path_id",
                table: "power_path_audit_trail",
                column: "power_path_id",
                principalTable: "power_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_power_path_audit_trail_AspNetUsers_actor_user_id",
                table: "power_path_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_power_path_audit_trail_Expressions_expression_id",
                table: "power_path_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_power_path_audit_trail_power_path_power_path_id",
                table: "power_path_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_power_path_audit_trail",
                table: "power_path_audit_trail");

            migrationBuilder.RenameTable(
                name: "power_path_audit_trail",
                newName: "PowerPathAuditTrail");

            migrationBuilder.RenameColumn(
                name: "timestamp",
                table: "PowerPathAuditTrail",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "action",
                table: "PowerPathAuditTrail",
                newName: "Action");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "PowerPathAuditTrail",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "power_path_id",
                table: "PowerPathAuditTrail",
                newName: "PowerPathId");

            migrationBuilder.RenameColumn(
                name: "expression_id",
                table: "PowerPathAuditTrail",
                newName: "ExpressionId");

            migrationBuilder.RenameColumn(
                name: "changed_properties",
                table: "PowerPathAuditTrail",
                newName: "ChangedProperties");

            migrationBuilder.RenameColumn(
                name: "actor_user_id",
                table: "PowerPathAuditTrail",
                newName: "ActorUserId");

            migrationBuilder.RenameIndex(
                name: "IX_power_path_audit_trail_power_path_id",
                table: "PowerPathAuditTrail",
                newName: "IX_PowerPathAuditTrail_PowerPathId");

            migrationBuilder.RenameIndex(
                name: "IX_power_path_audit_trail_expression_id",
                table: "PowerPathAuditTrail",
                newName: "IX_PowerPathAuditTrail_ExpressionId");

            migrationBuilder.RenameIndex(
                name: "IX_power_path_audit_trail_actor_user_id",
                table: "PowerPathAuditTrail",
                newName: "IX_PowerPathAuditTrail_ActorUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PowerPathAuditTrail",
                table: "PowerPathAuditTrail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PowerPathAuditTrail_AspNetUsers_ActorUserId",
                table: "PowerPathAuditTrail",
                column: "ActorUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PowerPathAuditTrail_Expressions_ExpressionId",
                table: "PowerPathAuditTrail",
                column: "ExpressionId",
                principalTable: "Expressions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PowerPathAuditTrail_power_path_PowerPathId",
                table: "PowerPathAuditTrail",
                column: "PowerPathId",
                principalTable: "power_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
