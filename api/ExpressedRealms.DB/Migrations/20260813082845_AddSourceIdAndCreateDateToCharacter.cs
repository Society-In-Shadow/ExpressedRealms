using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddSourceIdAndCreateDateToCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "create_date",
                table: "characters",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.Sql("""
                                 UPDATE public.characters
                                 SET create_date = CURRENT_TIMESTAMP
                                 WHERE create_date IS NULL;
                                 """);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "create_date",
                table: "characters",
                type: "timestamp with time zone",
                nullable: false,
                oldNullable: true);
            
            migrationBuilder.AddColumn<int>(
                name: "source_character_id",
                table: "characters",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_characters_source_character_id",
                table: "characters",
                column: "source_character_id");

            migrationBuilder.AddForeignKey(
                name: "fk_characters_characters_source_character_id",
                table: "characters",
                column: "source_character_id",
                principalTable: "characters",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_characters_characters_source_character_id",
                table: "characters");

            migrationBuilder.DropIndex(
                name: "ix_characters_source_character_id",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "create_date",
                table: "characters");

            migrationBuilder.DropColumn(
                name: "source_character_id",
                table: "characters");
        }
    }
}
