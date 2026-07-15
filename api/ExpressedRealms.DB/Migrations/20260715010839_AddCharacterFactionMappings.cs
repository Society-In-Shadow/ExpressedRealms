using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterFactionMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "character_faction_mappings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    character_id = table.Column<int>(type: "integer", nullable: false),
                    faction_rank_id = table.Column<int>(type: "integer", nullable: false),
                    approved_by_user_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    approval_reason = table.Column<string>(type: "character varying(20000)", maxLength: 20000, nullable: true),
                    character_notes = table.Column<string>(type: "character varying(20000)", maxLength: 20000, nullable: true),
                    request_promotion = table.Column<bool>(type: "boolean", nullable: false),
                    request_reason = table.Column<string>(type: "character varying(20000)", maxLength: 20000, nullable: true),
                    approval_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character_faction_mappings", x => x.id);
                    table.ForeignKey(
                        name: "fk_character_faction_mappings_characters_character_id",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_character_faction_mappings_faction_ranks_faction_rank_id",
                        column: x => x.faction_rank_id,
                        principalTable: "faction_ranks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_character_faction_mappings_users_approved_by_user_id",
                        column: x => x.approved_by_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_character_faction_mappings_approved_by_user_id",
                table: "character_faction_mappings",
                column: "approved_by_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_faction_mappings_character_id",
                table: "character_faction_mappings",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_faction_mappings_faction_rank_id",
                table: "character_faction_mappings",
                column: "faction_rank_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_faction_mappings");
        }
    }
}
