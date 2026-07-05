using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddCloneStuffToFactionsAndPowers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "other_fields",
                table: "powers",
                type: "character varying(20000)",
                maxLength: 20000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "limitation",
                table: "powers",
                type: "character varying(20000)",
                maxLength: 20000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "game_mechanic_effect",
                table: "powers",
                type: "character varying(20000)",
                maxLength: 20000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "powers",
                type: "character varying(20000)",
                maxLength: 20000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "cost",
                table: "powers",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "clone_batch_id",
                table: "powers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "clone_source_id",
                table: "powers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "clone_batch_id",
                table: "factions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "clone_source_id",
                table: "factions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "clone_batch_id",
                table: "faction_levels",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "clone_source_id",
                table: "faction_levels",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_powers_clone_source_id",
                table: "powers",
                column: "clone_source_id");

            migrationBuilder.CreateIndex(
                name: "ix_factions_clone_source_id",
                table: "factions",
                column: "clone_source_id");

            migrationBuilder.CreateIndex(
                name: "ix_faction_levels_clone_source_id",
                table: "faction_levels",
                column: "clone_source_id");

            migrationBuilder.AddForeignKey(
                name: "fk_faction_levels_faction_levels_clone_source_id",
                table: "faction_levels",
                column: "clone_source_id",
                principalTable: "faction_levels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_factions_factions_clone_source_id",
                table: "factions",
                column: "clone_source_id",
                principalTable: "factions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_powers_powers_clone_source_id",
                table: "powers",
                column: "clone_source_id",
                principalTable: "powers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_faction_levels_faction_levels_clone_source_id",
                table: "faction_levels");

            migrationBuilder.DropForeignKey(
                name: "fk_factions_factions_clone_source_id",
                table: "factions");

            migrationBuilder.DropForeignKey(
                name: "fk_powers_powers_clone_source_id",
                table: "powers");

            migrationBuilder.DropIndex(
                name: "ix_powers_clone_source_id",
                table: "powers");

            migrationBuilder.DropIndex(
                name: "ix_factions_clone_source_id",
                table: "factions");

            migrationBuilder.DropIndex(
                name: "ix_faction_levels_clone_source_id",
                table: "faction_levels");

            migrationBuilder.DropColumn(
                name: "clone_batch_id",
                table: "powers");

            migrationBuilder.DropColumn(
                name: "clone_source_id",
                table: "powers");

            migrationBuilder.DropColumn(
                name: "clone_batch_id",
                table: "factions");

            migrationBuilder.DropColumn(
                name: "clone_source_id",
                table: "factions");

            migrationBuilder.DropColumn(
                name: "clone_batch_id",
                table: "faction_levels");

            migrationBuilder.DropColumn(
                name: "clone_source_id",
                table: "faction_levels");

            migrationBuilder.AlterColumn<string>(
                name: "other_fields",
                table: "powers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20000)",
                oldMaxLength: 20000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "limitation",
                table: "powers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20000)",
                oldMaxLength: 20000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "game_mechanic_effect",
                table: "powers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20000)",
                oldMaxLength: 20000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "powers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20000)",
                oldMaxLength: 20000);

            migrationBuilder.AlterColumn<string>(
                name: "cost",
                table: "powers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250,
                oldNullable: true);
        }
    }
}
