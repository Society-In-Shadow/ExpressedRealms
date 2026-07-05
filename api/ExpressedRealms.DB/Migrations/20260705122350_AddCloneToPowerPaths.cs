using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddCloneToPowerPaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "clone_batch_id",
                table: "power_paths",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "clone_source_id",
                table: "power_paths",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_power_paths_clone_source_id",
                table: "power_paths",
                column: "clone_source_id");

            migrationBuilder.AddForeignKey(
                name: "fk_power_paths_power_paths_clone_source_id",
                table: "power_paths",
                column: "clone_source_id",
                principalTable: "power_paths",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_power_paths_power_paths_clone_source_id",
                table: "power_paths");

            migrationBuilder.DropIndex(
                name: "ix_power_paths_clone_source_id",
                table: "power_paths");

            migrationBuilder.DropColumn(
                name: "clone_batch_id",
                table: "power_paths");

            migrationBuilder.DropColumn(
                name: "clone_source_id",
                table: "power_paths");
        }
    }
}
