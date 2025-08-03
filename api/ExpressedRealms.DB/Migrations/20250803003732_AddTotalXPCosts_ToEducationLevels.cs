using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalXPCosts_ToEducationLevels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "total_general_xp_cost",
                table: "knowledge_education_level",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "total_unknown_xp_cost",
                table: "knowledge_education_level",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_general_xp_cost",
                table: "knowledge_education_level");

            migrationBuilder.DropColumn(
                name: "total_unknown_xp_cost",
                table: "knowledge_education_level");
        }
    }
}
