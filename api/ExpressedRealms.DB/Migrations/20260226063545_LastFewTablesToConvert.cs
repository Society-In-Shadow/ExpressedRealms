using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class LastFewTablesToConvert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_player_audit_trail_asp_net_users_actor_user_id",
                table: "Player_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "fk_player_audit_trail_players_player_id",
                table: "Player_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "fk_user_audit_trail_asp_net_users_actor_user_id",
                table: "User_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "fk_user_audit_trail_asp_net_users_user_id",
                table: "User_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_audit_trail_asp_net_roles_role_id",
                table: "UserRoles_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_audit_trail_asp_net_users_actor_user_id",
                table: "UserRoles_AuditTrail");

            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_audit_trail_asp_net_users_mapping_user_id",
                table: "UserRoles_AuditTrail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_roles_audit_trail",
                table: "UserRoles_AuditTrail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_audit_trail",
                table: "User_AuditTrail");

            migrationBuilder.DropPrimaryKey(
                name: "pk_player_audit_trail",
                table: "Player_AuditTrail");

            migrationBuilder.RenameTable(
                name: "UserRoles_AuditTrail",
                newName: "user_role_audit_trails");

            migrationBuilder.RenameTable(
                name: "User_AuditTrail",
                newName: "user_audit_trails");

            migrationBuilder.RenameTable(
                name: "Player_AuditTrail",
                newName: "player_audit_trails");

            migrationBuilder.RenameIndex(
                name: "ix_user_roles_audit_trail_role_id",
                table: "user_role_audit_trails",
                newName: "ix_user_role_audit_trails_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_roles_audit_trail_mapping_user_id",
                table: "user_role_audit_trails",
                newName: "ix_user_role_audit_trails_mapping_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_roles_audit_trail_actor_user_id",
                table: "user_role_audit_trails",
                newName: "ix_user_role_audit_trails_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_audit_trail_user_id",
                table: "user_audit_trails",
                newName: "ix_user_audit_trails_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_audit_trail_actor_user_id",
                table: "user_audit_trails",
                newName: "ix_user_audit_trails_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_player_audit_trail_player_id",
                table: "player_audit_trails",
                newName: "ix_player_audit_trails_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_player_audit_trail_actor_user_id",
                table: "player_audit_trails",
                newName: "ix_player_audit_trails_actor_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_role_audit_trails",
                table: "user_role_audit_trails",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_audit_trails",
                table: "user_audit_trails",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_player_audit_trails",
                table: "player_audit_trails",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_player_audit_trails_players_player_id",
                table: "player_audit_trails",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_audit_trails_users_actor_user_id",
                table: "player_audit_trails",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_audit_trails_asp_net_users_actor_user_id",
                table: "user_audit_trails",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_audit_trails_asp_net_users_user_id",
                table: "user_audit_trails",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_audit_trails_asp_net_users_actor_user_id",
                table: "user_role_audit_trails",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_audit_trails_asp_net_users_mapping_user_id",
                table: "user_role_audit_trails",
                column: "mapping_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_role_audit_trails_roles_role_id",
                table: "user_role_audit_trails",
                column: "role_id",
                principalTable: "AspNetRoles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_player_audit_trails_players_player_id",
                table: "player_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_player_audit_trails_users_actor_user_id",
                table: "player_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_user_audit_trails_asp_net_users_actor_user_id",
                table: "user_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_user_audit_trails_asp_net_users_user_id",
                table: "user_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_audit_trails_asp_net_users_actor_user_id",
                table: "user_role_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_audit_trails_asp_net_users_mapping_user_id",
                table: "user_role_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_user_role_audit_trails_roles_role_id",
                table: "user_role_audit_trails");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_role_audit_trails",
                table: "user_role_audit_trails");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_audit_trails",
                table: "user_audit_trails");

            migrationBuilder.DropPrimaryKey(
                name: "pk_player_audit_trails",
                table: "player_audit_trails");

            migrationBuilder.RenameTable(
                name: "user_role_audit_trails",
                newName: "UserRoles_AuditTrail");

            migrationBuilder.RenameTable(
                name: "user_audit_trails",
                newName: "User_AuditTrail");

            migrationBuilder.RenameTable(
                name: "player_audit_trails",
                newName: "Player_AuditTrail");

            migrationBuilder.RenameIndex(
                name: "ix_user_role_audit_trails_role_id",
                table: "UserRoles_AuditTrail",
                newName: "ix_user_roles_audit_trail_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_role_audit_trails_mapping_user_id",
                table: "UserRoles_AuditTrail",
                newName: "ix_user_roles_audit_trail_mapping_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_role_audit_trails_actor_user_id",
                table: "UserRoles_AuditTrail",
                newName: "ix_user_roles_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_audit_trails_user_id",
                table: "User_AuditTrail",
                newName: "ix_user_audit_trail_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_audit_trails_actor_user_id",
                table: "User_AuditTrail",
                newName: "ix_user_audit_trail_actor_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_player_audit_trails_player_id",
                table: "Player_AuditTrail",
                newName: "ix_player_audit_trail_player_id");

            migrationBuilder.RenameIndex(
                name: "ix_player_audit_trails_actor_user_id",
                table: "Player_AuditTrail",
                newName: "ix_player_audit_trail_actor_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_roles_audit_trail",
                table: "UserRoles_AuditTrail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_audit_trail",
                table: "User_AuditTrail",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_player_audit_trail",
                table: "Player_AuditTrail",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_player_audit_trail_asp_net_users_actor_user_id",
                table: "Player_AuditTrail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_player_audit_trail_players_player_id",
                table: "Player_AuditTrail",
                column: "player_id",
                principalTable: "players",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_audit_trail_asp_net_users_actor_user_id",
                table: "User_AuditTrail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_audit_trail_asp_net_users_user_id",
                table: "User_AuditTrail",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_audit_trail_asp_net_roles_role_id",
                table: "UserRoles_AuditTrail",
                column: "role_id",
                principalTable: "AspNetRoles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_audit_trail_asp_net_users_actor_user_id",
                table: "UserRoles_AuditTrail",
                column: "actor_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_audit_trail_asp_net_users_mapping_user_id",
                table: "UserRoles_AuditTrail",
                column: "mapping_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
