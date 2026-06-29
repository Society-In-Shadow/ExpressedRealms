using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddFactionLevelTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_contacts_factions_faction_id",
                table: "contacts");

            migrationBuilder.DropIndex(
                name: "ix_contacts_faction_id",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "faction_id",
                table: "contacts");

            migrationBuilder.CreateTable(
                name: "faction_rank",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_faction_rank", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "faction_level",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    faction_id = table.Column<int>(type: "integer", nullable: false),
                    faction_rank_id = table.Column<int>(type: "integer", nullable: false),
                    knowledge_id = table.Column<int>(type: "integer", nullable: true),
                    knowledge_level_id = table.Column<int>(type: "integer", nullable: true),
                    specialization = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_faction_level", x => x.id);
                    table.ForeignKey(
                        name: "fk_faction_level_faction_rank_faction_rank_id",
                        column: x => x.faction_rank_id,
                        principalTable: "faction_rank",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_faction_level_factions_faction_id",
                        column: x => x.faction_id,
                        principalTable: "factions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_faction_level_knowledge_education_levels_knowledge_level_id",
                        column: x => x.knowledge_level_id,
                        principalTable: "knowledge_education_levels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_faction_level_knowledges_knowledge_id",
                        column: x => x.knowledge_id,
                        principalTable: "knowledges",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "faction_level_audit_trail",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    faction_level_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actor_user_id = table.Column<string>(type: "text", nullable: false),
                    changed_properties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_faction_level_audit_trail", x => x.id);
                    table.ForeignKey(
                        name: "fk_faction_level_audit_trail_faction_level_faction_level_id",
                        column: x => x.faction_level_id,
                        principalTable: "faction_level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_faction_level_audit_trail_users_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "faction_rank",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Basic" },
                    { 2, "Intermediate" },
                    { 3, "Advance" },
                    { 4, "Supreme" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_faction_level_faction_id",
                table: "faction_level",
                column: "faction_id");

            migrationBuilder.CreateIndex(
                name: "ix_faction_level_faction_rank_id",
                table: "faction_level",
                column: "faction_rank_id");

            migrationBuilder.CreateIndex(
                name: "ix_faction_level_knowledge_id",
                table: "faction_level",
                column: "knowledge_id");

            migrationBuilder.CreateIndex(
                name: "ix_faction_level_knowledge_level_id",
                table: "faction_level",
                column: "knowledge_level_id");

            migrationBuilder.CreateIndex(
                name: "ix_faction_level_audit_trail_actor_user_id",
                table: "faction_level_audit_trail",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_faction_level_audit_trail_faction_level_id",
                table: "faction_level_audit_trail",
                column: "faction_level_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "faction_level_audit_trail");

            migrationBuilder.DropTable(
                name: "faction_level");

            migrationBuilder.DropTable(
                name: "faction_rank");

            migrationBuilder.AddColumn<int>(
                name: "faction_id",
                table: "contacts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_contacts_faction_id",
                table: "contacts",
                column: "faction_id");

            migrationBuilder.AddForeignKey(
                name: "fk_contacts_factions_faction_id",
                table: "contacts",
                column: "faction_id",
                principalTable: "factions",
                principalColumn: "id");
        }
    }
}
