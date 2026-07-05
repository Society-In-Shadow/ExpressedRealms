using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddExpressionAddCloneFieldsToPowerPrerequisites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "clone_batch_id",
                table: "power_prerequisites",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "clone_source_id",
                table: "power_prerequisites",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_power_prerequisites_clone_source_id",
                table: "power_prerequisites",
                column: "clone_source_id");

            migrationBuilder.AddForeignKey(
                name: "fk_power_prerequisites_power_prerequisites_clone_source_id",
                table: "power_prerequisites",
                column: "clone_source_id",
                principalTable: "power_prerequisites",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_power_prerequisites_power_prerequisites_clone_source_id",
                table: "power_prerequisites");

            migrationBuilder.DropIndex(
                name: "ix_power_prerequisites_clone_source_id",
                table: "power_prerequisites");

            migrationBuilder.DropColumn(
                name: "clone_batch_id",
                table: "power_prerequisites");

            migrationBuilder.DropColumn(
                name: "clone_source_id",
                table: "power_prerequisites");
        }
    }
}
