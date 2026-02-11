using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckinTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "checkin",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    player_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkin", x => x.id);
                    table.ForeignKey(
                        name: "FK_checkin_Players_player_id",
                        column: x => x.player_id,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_checkin_event_event_id",
                        column: x => x.event_id,
                        principalTable: "event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "checkin_stage",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkin_stage", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "checkin_audit_trail",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    checkin_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actor_user_id = table.Column<string>(type: "text", nullable: false),
                    changed_properties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkin_audit_trail", x => x.id);
                    table.ForeignKey(
                        name: "FK_checkin_audit_trail_AspNetUsers_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_checkin_audit_trail_checkin_checkin_id",
                        column: x => x.checkin_id,
                        principalTable: "checkin",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "checkin_question_response",
                columns: table => new
                {
                    checkin_id = table.Column<int>(type: "integer", nullable: false),
                    event_question_id = table.Column<int>(type: "integer", nullable: false),
                    response = table.Column<JsonDocument>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkin_question_response", x => new { x.checkin_id, x.event_question_id });
                    table.ForeignKey(
                        name: "FK_checkin_question_response_checkin_checkin_id",
                        column: x => x.checkin_id,
                        principalTable: "checkin",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_checkin_question_response_event_question_event_question_id",
                        column: x => x.event_question_id,
                        principalTable: "event_question",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "checkin_stage_mapping",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    checkin_id = table.Column<int>(type: "integer", nullable: false),
                    checkin_stage_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    approver_user_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkin_stage_mapping", x => x.id);
                    table.ForeignKey(
                        name: "FK_checkin_stage_mapping_AspNetUsers_approver_user_id",
                        column: x => x.approver_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_checkin_stage_mapping_checkin_checkin_id",
                        column: x => x.checkin_id,
                        principalTable: "checkin",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_checkin_stage_mapping_checkin_stage_checkin_stage_id",
                        column: x => x.checkin_stage_id,
                        principalTable: "checkin_stage",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "checkin_event_question_response_audit_trail",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    checkin_id = table.Column<int>(type: "integer", nullable: false),
                    event_question_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actor_user_id = table.Column<string>(type: "text", nullable: false),
                    changed_properties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkin_event_question_response_audit_trail", x => x.id);
                    table.ForeignKey(
                        name: "FK_checkin_event_question_response_audit_trail_AspNetUsers_act~",
                        column: x => x.actor_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_checkin_event_question_response_audit_trail_checkin_checkin~",
                        column: x => x.checkin_id,
                        principalTable: "checkin",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_checkin_event_question_response_audit_trail_checkin_questio~",
                        columns: x => new { x.event_question_id, x.checkin_id },
                        principalTable: "checkin_question_response",
                        principalColumns: new[] { "checkin_id", "event_question_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_checkin_event_question_response_audit_trail_event_question_~",
                        column: x => x.event_question_id,
                        principalTable: "event_question",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "checkin_stage",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 1, "All HR Questions have been answered and XP has been assigned out", "SHQ Approval" },
                    { 2, "GO has reviewed the character and approved it to good for play.", "GO Approval" },
                    { 3, "SHQ has received that it needs to print and ready the CRB.", "CRB Creation" },
                    { 4, "Player can now stop by SHQ  to pick up the CRB", "CRB Read For Pickup" },
                    { 5, "Player has picked up the CRB and verified that it's good to go", "CRB Picked Up" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_checkin_event_id",
                table: "checkin",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_checkin_player_id",
                table: "checkin",
                column: "player_id");

            migrationBuilder.CreateIndex(
                name: "IX_checkin_audit_trail_actor_user_id",
                table: "checkin_audit_trail",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_checkin_audit_trail_checkin_id",
                table: "checkin_audit_trail",
                column: "checkin_id");

            migrationBuilder.CreateIndex(
                name: "IX_checkin_event_question_response_audit_trail_actor_user_id",
                table: "checkin_event_question_response_audit_trail",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_checkin_event_question_response_audit_trail_checkin_id",
                table: "checkin_event_question_response_audit_trail",
                column: "checkin_id");

            migrationBuilder.CreateIndex(
                name: "IX_checkin_event_question_response_audit_trail_event_question_~",
                table: "checkin_event_question_response_audit_trail",
                columns: new[] { "event_question_id", "checkin_id" });

            migrationBuilder.CreateIndex(
                name: "IX_checkin_question_response_event_question_id",
                table: "checkin_question_response",
                column: "event_question_id");

            migrationBuilder.CreateIndex(
                name: "IX_checkin_stage_mapping_approver_user_id",
                table: "checkin_stage_mapping",
                column: "approver_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_checkin_stage_mapping_checkin_id",
                table: "checkin_stage_mapping",
                column: "checkin_id");

            migrationBuilder.CreateIndex(
                name: "IX_checkin_stage_mapping_checkin_stage_id",
                table: "checkin_stage_mapping",
                column: "checkin_stage_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "checkin_audit_trail");

            migrationBuilder.DropTable(
                name: "checkin_event_question_response_audit_trail");

            migrationBuilder.DropTable(
                name: "checkin_stage_mapping");

            migrationBuilder.DropTable(
                name: "checkin_question_response");

            migrationBuilder.DropTable(
                name: "checkin_stage");

            migrationBuilder.DropTable(
                name: "checkin");
        }
    }
}
