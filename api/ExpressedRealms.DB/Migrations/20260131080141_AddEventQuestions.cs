using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddEventQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "question_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    is_customizable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "event_question",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    question = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    question_type_id = table.Column<int>(type: "integer", nullable: false),
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_question", x => x.id);
                    table.ForeignKey(
                        name: "FK_event_question_event_event_id",
                        column: x => x.event_id,
                        principalTable: "event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_event_question_question_type_question_type_id",
                        column: x => x.question_type_id,
                        principalTable: "question_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "event_question_audit_trail",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    event_question_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actor_user_id = table.Column<string>(type: "text", nullable: false),
                    changed_properties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_question_audit_trail", x => x.id);
                    table.ForeignKey(
                        name: "FK_event_question_audit_trail_AspNetUsers_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_event_question_audit_trail_event_question_event_question_id",
                        column: x => x.event_question_id,
                        principalTable: "event_question",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "question_type",
                columns: new[] { "id", "is_customizable", "is_default", "name" },
                values: new object[,]
                {
                    { 1, false, true, "Is Minor" },
                    { 2, false, true, "Player Badge Number" },
                    { 3, true, false, "Text" },
                    { 4, true, false, "Checkbox" },
                    { 5, false, true, "Is New Player" },
                    { 6, false, true, "Brought New Player" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_event_question_event_id",
                table: "event_question",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_question_question_type_id",
                table: "event_question",
                column: "question_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_question_audit_trail_actor_user_id",
                table: "event_question_audit_trail",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_question_audit_trail_event_question_id",
                table: "event_question_audit_trail",
                column: "event_question_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "event_question_audit_trail");

            migrationBuilder.DropTable(
                name: "event_question");

            migrationBuilder.DropTable(
                name: "question_type");
        }
    }
}
