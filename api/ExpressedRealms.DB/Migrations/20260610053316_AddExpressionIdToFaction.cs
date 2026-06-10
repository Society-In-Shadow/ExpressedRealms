using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddExpressionIdToFaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "faction_type_id",
                table: "factions",
                newName: "expression_id");

            migrationBuilder.CreateIndex(
                name: "ix_factions_expression_id",
                table: "factions",
                column: "expression_id");

            migrationBuilder.AddForeignKey(
                name: "fk_factions_expressions_expression_id",
                table: "factions",
                column: "expression_id",
                principalTable: "expressions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_factions_expressions_expression_id",
                table: "factions");

            migrationBuilder.DropIndex(
                name: "ix_factions_expression_id",
                table: "factions");

            migrationBuilder.RenameColumn(
                name: "expression_id",
                table: "factions",
                newName: "faction_type_id");
        }
    }
}
