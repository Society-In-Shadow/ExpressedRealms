using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class TweakPermissionRoleTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_mapping_audit_trail_permission_permission_id",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.AlterColumn<int>(
                name: "permission_id",
                table: "role_permission_mapping_audit_trail",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "permission",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_mapping_audit_trail_permission_permission_id",
                table: "role_permission_mapping_audit_trail",
                column: "permission_id",
                principalTable: "permission",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_mapping_audit_trail_permission_permission_id",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.AlterColumn<int>(
                name: "permission_id",
                table: "role_permission_mapping_audit_trail",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "permission",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_mapping_audit_trail_permission_permission_id",
                table: "role_permission_mapping_audit_trail",
                column: "permission_id",
                principalTable: "permission",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
