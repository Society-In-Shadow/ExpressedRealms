using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddExpressionAddCloneFieldsToExpressionSections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "clone_batch_id",
                table: "expression_sections",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "clone_source_id",
                table: "expression_sections",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_expression_sections_clone_source_id",
                table: "expression_sections",
                column: "clone_source_id");

            migrationBuilder.AddForeignKey(
                name: "fk_expression_sections_expression_sections_clone_source_id",
                table: "expression_sections",
                column: "clone_source_id",
                principalTable: "expression_sections",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_expression_sections_expression_sections_clone_source_id",
                table: "expression_sections");

            migrationBuilder.DropIndex(
                name: "ix_expression_sections_clone_source_id",
                table: "expression_sections");

            migrationBuilder.DropColumn(
                name: "clone_batch_id",
                table: "expression_sections");

            migrationBuilder.DropColumn(
                name: "clone_source_id",
                table: "expression_sections");
        }
    }
}
