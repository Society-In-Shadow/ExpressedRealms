using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressedRealms.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddStorageOfSecondaryStatsAfterPrintingCRB2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_checkin_secondary_stat_checkins_checkin_id",
                table: "checkin_secondary_stat");

            migrationBuilder.DropPrimaryKey(
                name: "pk_checkin_secondary_stat",
                table: "checkin_secondary_stat");

            migrationBuilder.RenameTable(
                name: "checkin_secondary_stat",
                newName: "checkin_secondary_stats");

            migrationBuilder.RenameIndex(
                name: "ix_checkin_secondary_stat_checkin_id",
                table: "checkin_secondary_stats",
                newName: "ix_checkin_secondary_stats_checkin_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_checkin_secondary_stats",
                table: "checkin_secondary_stats",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_secondary_stats_checkins_checkin_id",
                table: "checkin_secondary_stats",
                column: "checkin_id",
                principalTable: "checkins",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_checkin_secondary_stats_checkins_checkin_id",
                table: "checkin_secondary_stats");

            migrationBuilder.DropPrimaryKey(
                name: "pk_checkin_secondary_stats",
                table: "checkin_secondary_stats");

            migrationBuilder.RenameTable(
                name: "checkin_secondary_stats",
                newName: "checkin_secondary_stat");

            migrationBuilder.RenameIndex(
                name: "ix_checkin_secondary_stats_checkin_id",
                table: "checkin_secondary_stat",
                newName: "ix_checkin_secondary_stat_checkin_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_checkin_secondary_stat",
                table: "checkin_secondary_stat",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_checkin_secondary_stat_checkins_checkin_id",
                table: "checkin_secondary_stat",
                column: "checkin_id",
                principalTable: "checkins",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
