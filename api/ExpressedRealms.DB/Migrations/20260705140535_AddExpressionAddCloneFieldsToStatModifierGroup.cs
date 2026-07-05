using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddExpressionAddCloneFieldsToStatModifierGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "clone_batch_id",
                table: "stat_modifier_groups",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "clone_source_id",
                table: "stat_modifier_groups",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_stat_modifier_groups_clone_source_id",
                table: "stat_modifier_groups",
                column: "clone_source_id");

            migrationBuilder.AddForeignKey(
                name: "fk_stat_modifier_groups_stat_modifier_groups_clone_source_id",
                table: "stat_modifier_groups",
                column: "clone_source_id",
                principalTable: "stat_modifier_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_stat_modifier_groups_stat_modifier_groups_clone_source_id",
                table: "stat_modifier_groups");

            migrationBuilder.DropIndex(
                name: "ix_stat_modifier_groups_clone_source_id",
                table: "stat_modifier_groups");

            migrationBuilder.DropColumn(
                name: "clone_batch_id",
                table: "stat_modifier_groups");

            migrationBuilder.DropColumn(
                name: "clone_source_id",
                table: "stat_modifier_groups");
        }
    }
}
