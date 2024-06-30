using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddExpressionIdToCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpressionId",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 3);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_ExpressionId",
                table: "Characters",
                column: "ExpressionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Expressions_ExpressionId",
                table: "Characters",
                column: "ExpressionId",
                principalTable: "Expressions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Expressions_ExpressionId",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_ExpressionId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ExpressionId",
                table: "Characters");
        }
    }
}
