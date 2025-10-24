using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddEventTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "event",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    start_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    location = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    website_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    website_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    additional_notes = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    con_experience = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "event_schedule_item",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_schedule_item", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "event_audit_trail",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actor_user_id = table.Column<string>(type: "text", nullable: false),
                    changed_properties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_audit_trail", x => x.id);
                    table.ForeignKey(
                        name: "FK_event_audit_trail_AspNetUsers_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_event_audit_trail_event_event_id",
                        column: x => x.event_id,
                        principalTable: "event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "event_schedule_item_audit_trail",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    event_schedule_item_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actor_user_id = table.Column<string>(type: "text", nullable: false),
                    changed_properties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_schedule_item_audit_trail", x => x.id);
                    table.ForeignKey(
                        name: "FK_event_schedule_item_audit_trail_AspNetUsers_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_event_schedule_item_audit_trail_event_EventId",
                        column: x => x.EventId,
                        principalTable: "event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_event_schedule_item_audit_trail_event_schedule_item_event_s~",
                        column: x => x.event_schedule_item_id,
                        principalTable: "event_schedule_item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_event_audit_trail_actor_user_id",
                table: "event_audit_trail",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_audit_trail_event_id",
                table: "event_audit_trail",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_schedule_item_audit_trail_actor_user_id",
                table: "event_schedule_item_audit_trail",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_schedule_item_audit_trail_event_schedule_item_id",
                table: "event_schedule_item_audit_trail",
                column: "event_schedule_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_schedule_item_audit_trail_EventId",
                table: "event_schedule_item_audit_trail",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "event_audit_trail");

            migrationBuilder.DropTable(
                name: "event_schedule_item_audit_trail");

            migrationBuilder.DropTable(
                name: "event");

            migrationBuilder.DropTable(
                name: "event_schedule_item");
        }
    }
}
