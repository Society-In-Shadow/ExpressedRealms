using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddContacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contact",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    character_id = table.Column<int>(type: "integer", nullable: false),
                    knowledge_id = table.Column<int>(type: "integer", nullable: false),
                    knowledge_level_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    notes = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    frequency = table.Column<byte>(type: "smallint", nullable: false),
                    spent_xp = table.Column<byte>(type: "smallint", nullable: false),
                    is_approved = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contact", x => x.id);
                    table.ForeignKey(
                        name: "FK_contact_Characters_character_id",
                        column: x => x.character_id,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_contact_knowledge_education_level_knowledge_level_id",
                        column: x => x.knowledge_level_id,
                        principalTable: "knowledge_education_level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_contact_knowledge_knowledge_id",
                        column: x => x.knowledge_id,
                        principalTable: "knowledge",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "contact_audit_trail",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    contact_id = table.Column<int>(type: "integer", nullable: true),
                    action = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actor_user_id = table.Column<string>(type: "text", nullable: false),
                    changed_properties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contact_audit_trail", x => x.id);
                    table.ForeignKey(
                        name: "FK_contact_audit_trail_AspNetUsers_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_contact_audit_trail_contact_contact_id",
                        column: x => x.contact_id,
                        principalTable: "contact",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_contact_character_id",
                table: "contact",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "IX_contact_knowledge_id",
                table: "contact",
                column: "knowledge_id");

            migrationBuilder.CreateIndex(
                name: "IX_contact_knowledge_level_id",
                table: "contact",
                column: "knowledge_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_contact_audit_trail_actor_user_id",
                table: "contact_audit_trail",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_contact_audit_trail_contact_id",
                table: "contact_audit_trail",
                column: "contact_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contact_audit_trail");

            migrationBuilder.DropTable(
                name: "contact");
        }
    }
}
