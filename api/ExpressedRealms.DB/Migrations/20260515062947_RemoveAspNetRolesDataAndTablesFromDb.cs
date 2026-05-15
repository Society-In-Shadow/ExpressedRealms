using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAspNetRolesDataAndTablesFromDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_role_audit_trails");

            migrationBuilder.Sql("""
                                 delete from public."AspNetUserRoles";
                                 delete from public."AspNetRoles";
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_role_audit_trails",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    actor_user_id = table.Column<string>(type: "text", nullable: false),
                    mapping_user_id = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<string>(type: "text", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    changed_properties = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_role_audit_trails", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_role_audit_trails_asp_net_users_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_role_audit_trails_asp_net_users_mapping_user_id",
                        column: x => x.mapping_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_role_audit_trails_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_role_audit_trails_actor_user_id",
                table: "user_role_audit_trails",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_role_audit_trails_mapping_user_id",
                table: "user_role_audit_trails",
                column: "mapping_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_role_audit_trails_role_id",
                table: "user_role_audit_trails",
                column: "role_id");
        }
    }
}
