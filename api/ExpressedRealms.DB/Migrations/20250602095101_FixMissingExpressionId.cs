using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class FixMissingExpressionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_power_path_Expressions_ExpressionId",
                table: "power_path");

            migrationBuilder.RenameColumn(
                name: "ExpressionId",
                table: "power_path",
                newName: "expression_id");

            migrationBuilder.RenameIndex(
                name: "IX_power_path_ExpressionId",
                table: "power_path",
                newName: "IX_power_path_expression_id");

            migrationBuilder.AddForeignKey(
                name: "FK_power_path_Expressions_expression_id",
                table: "power_path",
                column: "expression_id",
                principalTable: "Expressions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_power_path_Expressions_expression_id",
                table: "power_path");

            migrationBuilder.RenameColumn(
                name: "expression_id",
                table: "power_path",
                newName: "ExpressionId");

            migrationBuilder.RenameIndex(
                name: "IX_power_path_expression_id",
                table: "power_path",
                newName: "IX_power_path_ExpressionId");

            migrationBuilder.AddForeignKey(
                name: "FK_power_path_Expressions_ExpressionId",
                table: "power_path",
                column: "ExpressionId",
                principalTable: "Expressions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
