using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleAuditTablesToContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_role_audit_trail_role_role_id",
                table: "role_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_role_audit_trail_users_actor_user_id",
                table: "role_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permission_mapping_audit_trail_permission_permission_id",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permission_mapping_audit_trail_role_permission_mapping",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permission_mapping_audit_trail_role_role_id",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permission_mapping_audit_trail_users_actor_user_id",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_mapping_audit_trail_asp_net_users_actor_user_id",
                table: "user_role_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_mapping_audit_trail_asp_net_users_user_id",
                table: "user_role_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_mapping_audit_trail_role_role_id",
                table: "user_role_mapping_audit_trail");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_mapping_audit_trail_user_role_mapping_user_role_m",
                table: "user_role_mapping_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_role_mapping_audit_trail",
                table: "user_role_mapping_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_role_permission_mapping_audit_trail",
                table: "role_permission_mapping_audit_trail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_role_audit_trail",
                table: "role_audit_trail");

            migrationBuilder.RenameTable(
                name: "user_role_mapping_audit_trail",
                newName: "user_role_mapping_audit_trails");

            migrationBuilder.RenameTable(
                name: "role_permission_mapping_audit_trail",
                newName: "role_permission_mapping_audit_trails");

            migrationBuilder.RenameTable(
                name: "role_audit_trail",
                newName: "role_audit_trails");

            migrationBuilder.RenameIndex(
                name: "ix_user_role_mapping_audit_trail_user_role_mapping_id",
                table: "user_role_mapping_audit_trails",
                newName: "ix_user_role_mapping_audit_trails_user_role_mapping_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_role_mapping_audit_trail_user_id",
                table: "user_role_mapping_audit_trails",
                newName: "ix_user_role_mapping_audit_trails_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_role_mapping_audit_trail_role_id",
                table: "user_role_mapping_audit_trails",
                newName: "ix_user_role_mapping_audit_trails_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_role_mapping_audit_trail_actor_user_id",
                table: "user_role_mapping_audit_trails",
                newName: "ix_user_role_mapping_audit_trails_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_permission_mapping_audit_trail_role_permission_mapping",
                table: "role_permission_mapping_audit_trails",
                newName: "ix_role_permission_mapping_audit_trails_role_permission_mappin");

            migrationBuilder.RenameIndex(
                name: "ix_role_permission_mapping_audit_trail_role_id",
                table: "role_permission_mapping_audit_trails",
                newName: "ix_role_permission_mapping_audit_trails_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_permission_mapping_audit_trail_permission_id",
                table: "role_permission_mapping_audit_trails",
                newName: "ix_role_permission_mapping_audit_trails_permission_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_permission_mapping_audit_trail_actor_user_id",
                table: "role_permission_mapping_audit_trails",
                newName: "ix_role_permission_mapping_audit_trails_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_audit_trail_role_id",
                table: "role_audit_trails",
                newName: "ix_role_audit_trails_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_audit_trail_actor_user_id",
                table: "role_audit_trails",
                newName: "ix_role_audit_trails_actor_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_role_mapping_audit_trails",
                table: "user_role_mapping_audit_trails",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_role_permission_mapping_audit_trails",
                table: "role_permission_mapping_audit_trails",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_role_audit_trails",
                table: "role_audit_trails",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_role_audit_trails_role_role_id",
                table: "role_audit_trails",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_role_audit_trails_users_actor_user_id",
                table: "role_audit_trails",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permission_mapping_audit_trails_permission_permission_",
                table: "role_permission_mapping_audit_trails",
                column: "permission_id",
                principalTable: "permission",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permission_mapping_audit_trails_role_permission_mappin",
                table: "role_permission_mapping_audit_trails",
                column: "role_permission_mapping_id",
                principalTable: "role_permission_mapping",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permission_mapping_audit_trails_role_role_id",
                table: "role_permission_mapping_audit_trails",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permission_mapping_audit_trails_users_actor_user_id",
                table: "role_permission_mapping_audit_trails",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_mapping_audit_trails_asp_net_users_actor_user_id",
                table: "user_role_mapping_audit_trails",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_mapping_audit_trails_asp_net_users_user_id",
                table: "user_role_mapping_audit_trails",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_mapping_audit_trails_role_role_id",
                table: "user_role_mapping_audit_trails",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_mapping_audit_trails_user_role_mapping_user_role_",
                table: "user_role_mapping_audit_trails",
                column: "user_role_mapping_id",
                principalTable: "user_role_mapping",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_role_audit_trails_role_role_id",
                table: "role_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_role_audit_trails_users_actor_user_id",
                table: "role_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permission_mapping_audit_trails_permission_permission_",
                table: "role_permission_mapping_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permission_mapping_audit_trails_role_permission_mappin",
                table: "role_permission_mapping_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permission_mapping_audit_trails_role_role_id",
                table: "role_permission_mapping_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_role_permission_mapping_audit_trails_users_actor_user_id",
                table: "role_permission_mapping_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_mapping_audit_trails_asp_net_users_actor_user_id",
                table: "user_role_mapping_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_mapping_audit_trails_asp_net_users_user_id",
                table: "user_role_mapping_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_mapping_audit_trails_role_role_id",
                table: "user_role_mapping_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_mapping_audit_trails_user_role_mapping_user_role_",
                table: "user_role_mapping_audit_trails");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_role_mapping_audit_trails",
                table: "user_role_mapping_audit_trails");

            migrationBuilder.DropPrimaryKey(
                name: "pk_role_permission_mapping_audit_trails",
                table: "role_permission_mapping_audit_trails");

            migrationBuilder.DropPrimaryKey(
                name: "pk_role_audit_trails",
                table: "role_audit_trails");

            migrationBuilder.RenameTable(
                name: "user_role_mapping_audit_trails",
                newName: "user_role_mapping_audit_trail");

            migrationBuilder.RenameTable(
                name: "role_permission_mapping_audit_trails",
                newName: "role_permission_mapping_audit_trail");

            migrationBuilder.RenameTable(
                name: "role_audit_trails",
                newName: "role_audit_trail");

            migrationBuilder.RenameIndex(
                name: "ix_user_role_mapping_audit_trails_user_role_mapping_id",
                table: "user_role_mapping_audit_trail",
                newName: "ix_user_role_mapping_audit_trail_user_role_mapping_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_role_mapping_audit_trails_user_id",
                table: "user_role_mapping_audit_trail",
                newName: "ix_user_role_mapping_audit_trail_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_role_mapping_audit_trails_role_id",
                table: "user_role_mapping_audit_trail",
                newName: "ix_user_role_mapping_audit_trail_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_role_mapping_audit_trails_actor_user_id",
                table: "user_role_mapping_audit_trail",
                newName: "ix_user_role_mapping_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_permission_mapping_audit_trails_role_permission_mappin",
                table: "role_permission_mapping_audit_trail",
                newName: "ix_role_permission_mapping_audit_trail_role_permission_mapping");

            migrationBuilder.RenameIndex(
                name: "ix_role_permission_mapping_audit_trails_role_id",
                table: "role_permission_mapping_audit_trail",
                newName: "ix_role_permission_mapping_audit_trail_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_permission_mapping_audit_trails_permission_id",
                table: "role_permission_mapping_audit_trail",
                newName: "ix_role_permission_mapping_audit_trail_permission_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_permission_mapping_audit_trails_actor_user_id",
                table: "role_permission_mapping_audit_trail",
                newName: "ix_role_permission_mapping_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_audit_trails_role_id",
                table: "role_audit_trail",
                newName: "ix_role_audit_trail_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_role_audit_trails_actor_user_id",
                table: "role_audit_trail",
                newName: "ix_role_audit_trail_actor_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_role_mapping_audit_trail",
                table: "user_role_mapping_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_role_permission_mapping_audit_trail",
                table: "role_permission_mapping_audit_trail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_role_audit_trail",
                table: "role_audit_trail",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_role_audit_trail_role_role_id",
                table: "role_audit_trail",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_role_audit_trail_users_actor_user_id",
                table: "role_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permission_mapping_audit_trail_permission_permission_id",
                table: "role_permission_mapping_audit_trail",
                column: "permission_id",
                principalTable: "permission",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permission_mapping_audit_trail_role_permission_mapping",
                table: "role_permission_mapping_audit_trail",
                column: "role_permission_mapping_id",
                principalTable: "role_permission_mapping",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permission_mapping_audit_trail_role_role_id",
                table: "role_permission_mapping_audit_trail",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_role_permission_mapping_audit_trail_users_actor_user_id",
                table: "role_permission_mapping_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_mapping_audit_trail_asp_net_users_actor_user_id",
                table: "user_role_mapping_audit_trail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_mapping_audit_trail_asp_net_users_user_id",
                table: "user_role_mapping_audit_trail",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_mapping_audit_trail_role_role_id",
                table: "user_role_mapping_audit_trail",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_mapping_audit_trail_user_role_mapping_user_role_m",
                table: "user_role_mapping_audit_trail",
                column: "user_role_mapping_id",
                principalTable: "user_role_mapping",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
