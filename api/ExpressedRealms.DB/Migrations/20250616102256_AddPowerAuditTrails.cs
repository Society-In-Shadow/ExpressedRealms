using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddPowerAuditTrails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "power_audit_trail",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    power_path_id = table.Column<int>(type: "integer", nullable: false),
                    power_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actor_user_id = table.Column<string>(type: "text", nullable: false),
                    changed_properties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_power_audit_trail", x => x.id);
                    table.ForeignKey(
                        name: "FK_power_audit_trail_AspNetUsers_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_power_audit_trail_power_path_power_path_id",
                        column: x => x.power_path_id,
                        principalTable: "power_path",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_power_audit_trail_power_power_id",
                        column: x => x.power_id,
                        principalTable: "power",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_power_audit_trail_actor_user_id",
                table: "power_audit_trail",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_power_audit_trail_power_id",
                table: "power_audit_trail",
                column: "power_id");

            migrationBuilder.CreateIndex(
                name: "IX_power_audit_trail_power_path_id",
                table: "power_audit_trail",
                column: "power_path_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "power_audit_trail");
        }
    }
}
