using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddBlessingLevelTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "blessing_level",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    blessing_id = table.Column<int>(type: "integer", nullable: false),
                    level = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    xp_cost = table.Column<int>(type: "integer", nullable: false),
                    xp_gain = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blessing_level", x => x.id);
                    table.ForeignKey(
                        name: "FK_blessing_level_blessing_blessing_id",
                        column: x => x.blessing_id,
                        principalTable: "blessing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "blessing_level_audit_trail",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    blessing_id = table.Column<int>(type: "integer", nullable: false),
                    blessing_level_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actor_user_id = table.Column<string>(type: "text", nullable: false),
                    changed_properties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blessing_level_audit_trail", x => x.id);
                    table.ForeignKey(
                        name: "FK_blessing_level_audit_trail_AspNetUsers_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_blessing_level_audit_trail_blessing_blessing_id",
                        column: x => x.blessing_id,
                        principalTable: "blessing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_blessing_level_audit_trail_blessing_level_blessing_level_id",
                        column: x => x.blessing_level_id,
                        principalTable: "blessing_level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_blessing_level_blessing_id",
                table: "blessing_level",
                column: "blessing_id");

            migrationBuilder.CreateIndex(
                name: "IX_blessing_level_audit_trail_actor_user_id",
                table: "blessing_level_audit_trail",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_blessing_level_audit_trail_blessing_id",
                table: "blessing_level_audit_trail",
                column: "blessing_id");

            migrationBuilder.CreateIndex(
                name: "IX_blessing_level_audit_trail_blessing_level_id",
                table: "blessing_level_audit_trail",
                column: "blessing_level_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blessing_level_audit_trail");

            migrationBuilder.DropTable(
                name: "blessing_level");
        }
    }
}
