using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedXpTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assigned_xp_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assigned_xp_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "assigned_xp_mapping",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    character_id = table.Column<int>(type: "integer", nullable: false),
                    player_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    assigned_xp_type_id = table.Column<int>(type: "integer", nullable: false),
                    assigned_by_user_id = table.Column<string>(type: "text", nullable: false),
                    reason = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assigned_xp_mapping", x => x.id);
                    table.ForeignKey(
                        name: "FK_assigned_xp_mapping_AspNetUsers_assigned_by_user_id",
                        column: x => x.assigned_by_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_assigned_xp_mapping_Characters_character_id",
                        column: x => x.character_id,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_assigned_xp_mapping_Players_player_id",
                        column: x => x.player_id,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_assigned_xp_mapping_assigned_xp_type_assigned_xp_type_id",
                        column: x => x.assigned_xp_type_id,
                        principalTable: "assigned_xp_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_assigned_xp_mapping_event_event_id",
                        column: x => x.event_id,
                        principalTable: "event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "assigned_xp_type_audit_trail",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    assigned_xp_type_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actor_user_id = table.Column<string>(type: "text", nullable: false),
                    changed_properties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assigned_xp_type_audit_trail", x => x.id);
                    table.ForeignKey(
                        name: "FK_assigned_xp_type_audit_trail_AspNetUsers_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_assigned_xp_type_audit_trail_assigned_xp_type_assigned_xp_t~",
                        column: x => x.assigned_xp_type_id,
                        principalTable: "assigned_xp_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "assigned_xp_mapping_audit_trail",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    assigned_xp_mapping_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actor_user_id = table.Column<string>(type: "text", nullable: false),
                    changed_properties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assigned_xp_mapping_audit_trail", x => x.id);
                    table.ForeignKey(
                        name: "FK_assigned_xp_mapping_audit_trail_AspNetUsers_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_assigned_xp_mapping_audit_trail_assigned_xp_mapping_assigne~",
                        column: x => x.assigned_xp_mapping_id,
                        principalTable: "assigned_xp_mapping",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_assigned_xp_mapping_assigned_by_user_id",
                table: "assigned_xp_mapping",
                column: "assigned_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_assigned_xp_mapping_assigned_xp_type_id",
                table: "assigned_xp_mapping",
                column: "assigned_xp_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_assigned_xp_mapping_character_id",
                table: "assigned_xp_mapping",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "IX_assigned_xp_mapping_event_id",
                table: "assigned_xp_mapping",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_assigned_xp_mapping_player_id",
                table: "assigned_xp_mapping",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_assigned_xp_mapping_audit_trail_actor_user_id",
                table: "assigned_xp_mapping_audit_trail",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_assigned_xp_mapping_audit_trail_assigned_xp_mapping_id",
                table: "assigned_xp_mapping_audit_trail",
                column: "assigned_xp_mapping_id");

            migrationBuilder.CreateIndex(
                name: "IX_assigned_xp_type_audit_trail_actor_user_id",
                table: "assigned_xp_type_audit_trail",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_assigned_xp_type_audit_trail_assigned_xp_type_id",
                table: "assigned_xp_type_audit_trail",
                column: "assigned_xp_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assigned_xp_mapping_audit_trail");

            migrationBuilder.DropTable(
                name: "assigned_xp_type_audit_trail");

            migrationBuilder.DropTable(
                name: "assigned_xp_mapping");

            migrationBuilder.DropTable(
                name: "assigned_xp_type");
        }
    }
}
