using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddStatGroupMappingAuditTableAndTargetProgressionPathId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "target_progression_path_id",
                table: "stat_group_mappings",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "stat_group_mapping_audit_trail",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    stat_group_mapping_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actor_user_id = table.Column<string>(type: "text", nullable: false),
                    changed_properties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stat_group_mapping_audit_trail", x => x.id);
                    table.ForeignKey(
                        name: "fk_stat_group_mapping_audit_trail_stat_group_mappings_stat_gro",
                        column: x => x.stat_group_mapping_id,
                        principalTable: "stat_group_mappings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_stat_group_mapping_audit_trail_users_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_stat_group_mappings_target_progression_path_id",
                table: "stat_group_mappings",
                column: "target_progression_path_id");

            migrationBuilder.CreateIndex(
                name: "ix_stat_group_mapping_audit_trail_actor_user_id",
                table: "stat_group_mapping_audit_trail",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_stat_group_mapping_audit_trail_stat_group_mapping_id",
                table: "stat_group_mapping_audit_trail",
                column: "stat_group_mapping_id");

            migrationBuilder.AddForeignKey(
                name: "fk_stat_group_mappings_progression_path_target_progression_pat",
                table: "stat_group_mappings",
                column: "target_progression_path_id",
                principalTable: "progression_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_stat_group_mappings_progression_path_target_progression_pat",
                table: "stat_group_mappings");

            migrationBuilder.DropTable(
                name: "stat_group_mapping_audit_trail");

            migrationBuilder.DropIndex(
                name: "ix_stat_group_mappings_target_progression_path_id",
                table: "stat_group_mappings");

            migrationBuilder.DropColumn(
                name: "target_progression_path_id",
                table: "stat_group_mappings");
        }
    }
}
