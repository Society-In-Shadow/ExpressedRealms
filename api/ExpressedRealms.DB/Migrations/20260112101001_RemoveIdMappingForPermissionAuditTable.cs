using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIdMappingForPermissionAuditTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_mapping_audit_trail_role_permission_mapping~",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.AlterColumn<int>(
                name: "role_permission_mapping_id",
                table: "role_permission_mapping_audit_trail",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_mapping_audit_trail_role_permission_mapping~",
                table: "role_permission_mapping_audit_trail",
                column: "role_permission_mapping_id",
                principalTable: "role_permission_mapping",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_mapping_audit_trail_role_permission_mapping~",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.AlterColumn<int>(
                name: "role_permission_mapping_id",
                table: "role_permission_mapping_audit_trail",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_mapping_audit_trail_role_permission_mapping~",
                table: "role_permission_mapping_audit_trail",
                column: "role_permission_mapping_id",
                principalTable: "role_permission_mapping",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
