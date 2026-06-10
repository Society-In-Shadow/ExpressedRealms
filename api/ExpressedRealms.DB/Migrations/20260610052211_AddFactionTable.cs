using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddFactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "faction_id",
                table: "contacts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "factions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    faction_type_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    background = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_factions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "faction_audit_trails",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    faction_id = table.Column<int>(type: "integer", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    actor_user_id = table.Column<string>(type: "text", nullable: false),
                    changed_properties = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_faction_audit_trails", x => x.id);
                    table.ForeignKey(
                        name: "fk_faction_audit_trails_factions_faction_id",
                        column: x => x.faction_id,
                        principalTable: "factions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_faction_audit_trails_users_actor_user_id",
                        column: x => x.actor_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_contacts_faction_id",
                table: "contacts",
                column: "faction_id");

            migrationBuilder.CreateIndex(
                name: "ix_faction_audit_trails_actor_user_id",
                table: "faction_audit_trails",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_faction_audit_trails_faction_id",
                table: "faction_audit_trails",
                column: "faction_id");

            migrationBuilder.AddForeignKey(
                name: "fk_contacts_factions_faction_id",
                table: "contacts",
                column: "faction_id",
                principalTable: "factions",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_contacts_factions_faction_id",
                table: "contacts");

            migrationBuilder.DropTable(
                name: "faction_audit_trails");

            migrationBuilder.DropTable(
                name: "factions");

            migrationBuilder.DropIndex(
                name: "ix_contacts_faction_id",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "faction_id",
                table: "contacts");
        }
    }
}
