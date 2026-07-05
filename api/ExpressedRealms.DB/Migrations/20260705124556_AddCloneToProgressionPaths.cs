using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddCloneToProgressionPaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "clone_batch_id",
                table: "progression_path",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "clone_source_id",
                table: "progression_path",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_progression_path_clone_source_id",
                table: "progression_path",
                column: "clone_source_id");

            migrationBuilder.AddForeignKey(
                name: "fk_progression_path_progression_path_clone_source_id",
                table: "progression_path",
                column: "clone_source_id",
                principalTable: "progression_path",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_progression_path_progression_path_clone_source_id",
                table: "progression_path");

            migrationBuilder.DropIndex(
                name: "ix_progression_path_clone_source_id",
                table: "progression_path");

            migrationBuilder.DropColumn(
                name: "clone_batch_id",
                table: "progression_path");

            migrationBuilder.DropColumn(
                name: "clone_source_id",
                table: "progression_path");
        }
    }
}
