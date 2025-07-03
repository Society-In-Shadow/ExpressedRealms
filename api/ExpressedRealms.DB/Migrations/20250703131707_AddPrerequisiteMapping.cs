using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddPrerequisiteMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_power_prerequisite_power_power_prerequisite_prerequisite_id",
                table: "power_prerequisite_power");

            migrationBuilder.AddForeignKey(
                name: "FK_power_prerequisite_power_power_prerequisite_prerequisite_id",
                table: "power_prerequisite_power",
                column: "prerequisite_id",
                principalTable: "power_prerequisite",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_power_prerequisite_power_power_prerequisite_prerequisite_id",
                table: "power_prerequisite_power");

            migrationBuilder.AddForeignKey(
                name: "FK_power_prerequisite_power_power_prerequisite_prerequisite_id",
                table: "power_prerequisite_power",
                column: "prerequisite_id",
                principalTable: "power_prerequisite",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
