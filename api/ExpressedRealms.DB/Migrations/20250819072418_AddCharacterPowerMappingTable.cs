using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterPowerMappingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Xp",
                table: "power_level",
                newName: "xp");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "power_level",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "power_level",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "power_level",
                newName: "id");

            migrationBuilder.AddColumn<int>(
                name: "total_xp",
                table: "power_level",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "character_power_mapping",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    character_id = table.Column<int>(type: "integer", nullable: false),
                    power_id = table.Column<int>(type: "integer", nullable: false),
                    power_level_id = table.Column<int>(type: "integer", nullable: false),
                    notes = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_character_power_mapping", x => x.id);
                    table.ForeignKey(
                        name: "FK_character_power_mapping_Characters_character_id",
                        column: x => x.character_id,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_character_power_mapping_power_level_power_level_id",
                        column: x => x.power_level_id,
                        principalTable: "power_level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_character_power_mapping_power_power_id",
                        column: x => x.power_id,
                        principalTable: "power",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_character_power_mapping_character_id",
                table: "character_power_mapping",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "IX_character_power_mapping_power_id",
                table: "character_power_mapping",
                column: "power_id");

            migrationBuilder.CreateIndex(
                name: "IX_character_power_mapping_power_level_id",
                table: "character_power_mapping",
                column: "power_level_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_power_mapping");

            migrationBuilder.DropColumn(
                name: "total_xp",
                table: "power_level");

            migrationBuilder.RenameColumn(
                name: "xp",
                table: "power_level",
                newName: "Xp");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "power_level",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "power_level",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "power_level",
                newName: "Id");
        }
    }
}
