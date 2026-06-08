using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class RemovePathIdAndOrderIndexOnPowers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_power_audit_trails_power_paths_power_path_id",
                table: "power_audit_trails");

            migrationBuilder.DropForeignKey(
                name: "fk_power_path_power_mappings_powers_power_id",
                table: "power_path_power_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_powers_power_paths_power_path_id",
                table: "powers");

            migrationBuilder.DropIndex(
                name: "ix_powers_power_path_id",
                table: "powers");

            migrationBuilder.DropIndex(
                name: "ix_power_path_power_mappings_power_id",
                table: "power_path_power_mappings");

            migrationBuilder.DropIndex(
                name: "ix_power_audit_trails_power_path_id",
                table: "power_audit_trails");

            migrationBuilder.DropColumn(
                name: "order_index",
                table: "powers");

            migrationBuilder.DropColumn(
                name: "power_path_id",
                table: "powers");

            migrationBuilder.DropColumn(
                name: "power_path_id",
                table: "power_audit_trails");

            migrationBuilder.CreateIndex(
                name: "ix_power_path_power_mappings_power_id",
                table: "power_path_power_mappings",
                column: "power_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_power_path_power_mappings_powers_power_id",
                table: "power_path_power_mappings",
                column: "power_id",
                principalTable: "powers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_power_path_power_mappings_powers_power_id",
                table: "power_path_power_mappings");

            migrationBuilder.DropIndex(
                name: "ix_power_path_power_mappings_power_id",
                table: "power_path_power_mappings");

            migrationBuilder.AddColumn<int>(
                name: "order_index",
                table: "powers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "power_path_id",
                table: "powers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "power_path_id",
                table: "power_audit_trails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_powers_power_path_id",
                table: "powers",
                column: "power_path_id");

            migrationBuilder.CreateIndex(
                name: "ix_power_path_power_mappings_power_id",
                table: "power_path_power_mappings",
                column: "power_id");

            migrationBuilder.CreateIndex(
                name: "ix_power_audit_trails_power_path_id",
                table: "power_audit_trails",
                column: "power_path_id");

            migrationBuilder.AddForeignKey(
                name: "fk_power_audit_trails_power_paths_power_path_id",
                table: "power_audit_trails",
                column: "power_path_id",
                principalTable: "power_paths",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_power_path_power_mappings_powers_power_id",
                table: "power_path_power_mappings",
                column: "power_id",
                principalTable: "powers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_powers_power_paths_power_path_id",
                table: "powers",
                column: "power_path_id",
                principalTable: "power_paths",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
