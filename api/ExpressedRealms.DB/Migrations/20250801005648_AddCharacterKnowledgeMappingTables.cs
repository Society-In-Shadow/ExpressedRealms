using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterKnowledgeMappingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "character_knowledge_mapping",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    character_id = table.Column<int>(type: "integer", nullable: false),
                    knowledge_id = table.Column<int>(type: "integer", nullable: false),
                    knowledge_level_id = table.Column<int>(type: "integer", nullable: false),
                    notes = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_character_knowledge_mapping", x => x.id);
                    table.ForeignKey(
                        name: "FK_character_knowledge_mapping_Characters_character_id",
                        column: x => x.character_id,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_character_knowledge_mapping_knowledge_education_level_knowl~",
                        column: x => x.knowledge_level_id,
                        principalTable: "knowledge_education_level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_character_knowledge_mapping_knowledge_knowledge_level_id",
                        column: x => x.knowledge_level_id,
                        principalTable: "knowledge",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "character_knowledge_specialization",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    knowledge_mapping_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    notes = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_character_knowledge_specialization", x => x.id);
                    table.ForeignKey(
                        name: "FK_character_knowledge_specialization_character_knowledge_mapp~",
                        column: x => x.knowledge_mapping_id,
                        principalTable: "character_knowledge_mapping",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_character_knowledge_mapping_character_id",
                table: "character_knowledge_mapping",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "IX_character_knowledge_mapping_knowledge_level_id",
                table: "character_knowledge_mapping",
                column: "knowledge_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_character_knowledge_specialization_knowledge_mapping_id",
                table: "character_knowledge_specialization",
                column: "knowledge_mapping_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_knowledge_specialization");

            migrationBuilder.DropTable(
                name: "character_knowledge_mapping");
        }
    }
}
