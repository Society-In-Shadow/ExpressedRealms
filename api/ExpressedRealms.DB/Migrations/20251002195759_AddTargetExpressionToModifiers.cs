using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddTargetExpressionToModifiers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "target_expression_id",
                table: "stat_group_mapping",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_stat_group_mapping_target_expression_id",
                table: "stat_group_mapping",
                column: "target_expression_id");

            migrationBuilder.AddForeignKey(
                name: "FK_stat_group_mapping_expression_target_expression_id",
                table: "stat_group_mapping",
                column: "target_expression_id",
                principalTable: "expression",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_stat_group_mapping_expression_target_expression_id",
                table: "stat_group_mapping");

            migrationBuilder.DropIndex(
                name: "IX_stat_group_mapping_target_expression_id",
                table: "stat_group_mapping");

            migrationBuilder.DropColumn(
                name: "target_expression_id",
                table: "stat_group_mapping");
        }
    }
}
