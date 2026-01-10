using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoleAuditAndMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_mapping_role_role_id",
                table: "role_permission_mapping");

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_mapping_role_role_id",
                table: "role_permission_mapping",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_mapping_role_role_id",
                table: "role_permission_mapping");

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_mapping_role_role_id",
                table: "role_permission_mapping",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
