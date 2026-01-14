using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKeysOnRolePermissionMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_mapping_audit_trail_permission_permission_id",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_mapping_audit_trail_role_permission_mapping~",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_mapping_audit_trail_role_role_id",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_mapping_audit_trail_permission_permission_id",
                table: "role_permission_mapping_audit_trail",
                column: "permission_id",
                principalTable: "permission",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_mapping_audit_trail_role_permission_mapping~",
                table: "role_permission_mapping_audit_trail",
                column: "role_permission_mapping_id",
                principalTable: "role_permission_mapping",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_mapping_audit_trail_role_role_id",
                table: "role_permission_mapping_audit_trail",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_mapping_audit_trail_permission_permission_id",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_mapping_audit_trail_role_permission_mapping~",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_mapping_audit_trail_role_role_id",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_mapping_audit_trail_permission_permission_id",
                table: "role_permission_mapping_audit_trail",
                column: "permission_id",
                principalTable: "permission",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_mapping_audit_trail_role_permission_mapping~",
                table: "role_permission_mapping_audit_trail",
                column: "role_permission_mapping_id",
                principalTable: "role_permission_mapping",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_mapping_audit_trail_role_role_id",
                table: "role_permission_mapping_audit_trail",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
