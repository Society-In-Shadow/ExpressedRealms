using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddPowerToFactionLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "power_id",
                table: "faction_levels",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_faction_levels_power_id",
                table: "faction_levels",
                column: "power_id");

            migrationBuilder.AddForeignKey(
                name: "fk_faction_levels_powers_power_id",
                table: "faction_levels",
                column: "power_id",
                principalTable: "powers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_faction_levels_powers_power_id",
                table: "faction_levels");

            migrationBuilder.DropIndex(
                name: "ix_faction_levels_power_id",
                table: "faction_levels");

            migrationBuilder.DropColumn(
                name: "power_id",
                table: "faction_levels");
        }
    }
}
