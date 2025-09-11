using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterBlessingMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "character_blessing_mapping",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    character_id = table.Column<int>(type: "integer", nullable: false),
                    blessing_id = table.Column<int>(type: "integer", nullable: false),
                    blessing_level_id = table.Column<int>(type: "integer", nullable: false),
                    notes = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_character_blessing_mapping", x => x.id);
                    table.ForeignKey(
                        name: "FK_character_blessing_mapping_Characters_character_id",
                        column: x => x.character_id,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_character_blessing_mapping_blessing_blessing_id",
                        column: x => x.blessing_id,
                        principalTable: "blessing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_character_blessing_mapping_blessing_level_blessing_level_id",
                        column: x => x.blessing_level_id,
                        principalTable: "blessing_level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_character_blessing_mapping_blessing_id",
                table: "character_blessing_mapping",
                column: "blessing_id");

            migrationBuilder.CreateIndex(
                name: "IX_character_blessing_mapping_blessing_level_id",
                table: "character_blessing_mapping",
                column: "blessing_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_character_blessing_mapping_character_id",
                table: "character_blessing_mapping",
                column: "character_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_blessing_mapping");
        }
    }
}
